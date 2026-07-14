SELECT
    p.surname,
    p.name,
    COALESCE(SUM(od.price_with_discount * od.product_amount), 0) AS sum
FROM customer AS c
INNER JOIN person AS p
    ON p.id = c.person_id
LEFT JOIN customer_order AS co
    ON co.customer_id = c.person_id
LEFT JOIN order_details AS od
    ON od.customer_order_id = co.id
GROUP BY p.surname, p.name

UNION ALL

SELECT
    NULL AS surname,
    NULL AS name,
    COALESCE(SUM(od.price_with_discount * od.product_amount), 0) AS sum
FROM customer_order AS co
LEFT JOIN order_details AS od
    ON od.customer_order_id = co.id
WHERE co.customer_id IS NULL

ORDER BY sum, surname;
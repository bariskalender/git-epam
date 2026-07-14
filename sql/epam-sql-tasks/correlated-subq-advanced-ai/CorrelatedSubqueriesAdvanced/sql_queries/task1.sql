SELECT
    p.name,
    p.surname,
    AVG(order_total.total_amount) AS avg_purchase,
    SUM(order_total.total_amount) AS sum_purchase
FROM person AS p
JOIN customer AS c
    ON c.person_id = p.id
JOIN (
    SELECT
        co.id,
        co.customer_id,
        SUM(od.price_with_discount * od.product_amount) AS total_amount
    FROM customer_order AS co
    JOIN order_details AS od
        ON od.customer_order_id = co.id
    WHERE co.customer_id IS NOT NULL
    GROUP BY co.id, co.customer_id
) AS order_total
    ON order_total.customer_id = c.person_id
GROUP BY p.id, p.name, p.surname
HAVING AVG(order_total.total_amount) > 70
ORDER BY sum_purchase ASC, p.surname ASC;
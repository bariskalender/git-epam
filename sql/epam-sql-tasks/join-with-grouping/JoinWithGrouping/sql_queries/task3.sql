SELECT
    p.surname,
    p.name,
    p.birth_date,
    SUM(od.price_with_discount * od.product_amount) AS sum
FROM order_details AS od
INNER JOIN customer_order AS co
    ON co.id = od.customer_order_id
INNER JOIN customer AS c
    ON c.person_id = co.customer_id
INNER JOIN person AS p
    ON p.id = c.person_id
WHERE c.discount = 0
  AND date(co.operation_time) BETWEEN '2021-01-01' AND '2021-12-31'
GROUP BY p.surname, p.name, p.birth_date
ORDER BY sum, p.surname;
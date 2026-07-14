SELECT
    c.name AS city,
    SUM(od.price_with_discount * od.product_amount) AS income
FROM order_details AS od
INNER JOIN customer_order AS co
    ON co.id = od.customer_order_id
INNER JOIN supermarket AS s
    ON s.id = co.supermarket_id
INNER JOIN street AS st
    ON st.id = s.street_id
INNER JOIN city AS c
    ON c.id = st.city_id
WHERE date(co.operation_time) BETWEEN '2020-11-01' AND '2020-11-30'
GROUP BY c.name
ORDER BY income, c.name;
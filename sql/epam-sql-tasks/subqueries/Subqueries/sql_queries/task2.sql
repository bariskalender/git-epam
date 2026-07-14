SELECT
    pe.surname,
    pe.name,
    SUM(od.price_with_discount * od.product_amount) AS total_expenses
FROM customer_order AS co
JOIN customer AS c
    ON c.person_id = co.customer_id
JOIN person AS pe
    ON pe.id = c.person_id
JOIN order_details AS od
    ON od.customer_order_id = co.id
WHERE pe.birth_date BETWEEN '2000-01-01' AND '2010-12-31'
GROUP BY pe.surname, pe.name, pe.id
HAVING SUM(od.price_with_discount * od.product_amount) > (
    SELECT AVG(customer_total.total_expenses)
    FROM (
        SELECT
            SUM(od2.price_with_discount * od2.product_amount) AS total_expenses
        FROM customer_order AS co2
        JOIN customer AS c2
            ON c2.person_id = co2.customer_id
        JOIN order_details AS od2
            ON od2.customer_order_id = co2.id
        GROUP BY c2.person_id
    ) AS customer_total
)
ORDER BY total_expenses ASC, pe.surname ASC;
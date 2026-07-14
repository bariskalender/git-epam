SELECT
    pt.title,
    od.price
FROM order_details AS od
INNER JOIN product AS p
    ON p.id = od.product_id
INNER JOIN product_title AS pt
    ON pt.id = p.product_title_id
INNER JOIN customer_order AS co
    ON co.id = od.customer_order_id
INNER JOIN customer AS c
    ON c.person_id = co.customer_id
INNER JOIN person AS pe
    ON pe.id = c.person_id
WHERE pe.birth_date BETWEEN DATE '2000-01-01' AND DATE '2010-12-31'
ORDER BY pt.title, od.price;
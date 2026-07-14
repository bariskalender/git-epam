SELECT
    pc.name AS category,
    pt.title AS product
FROM product AS p
INNER JOIN product_title AS pt
    ON pt.id = p.product_title_id
INNER JOIN product_category AS pc
    ON pc.id = pt.product_category_id
LEFT JOIN order_details AS od
    ON od.product_id = p.id
WHERE od.product_id IS NULL
ORDER BY p.id;
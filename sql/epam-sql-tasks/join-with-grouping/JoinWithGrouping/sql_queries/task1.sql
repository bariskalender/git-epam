SELECT
    pc.name AS category,
    COUNT(p.id) AS count
FROM product AS p
INNER JOIN product_title AS pt
    ON pt.id = p.product_title_id
INNER JOIN product_category AS pc
    ON pc.id = pt.product_category_id
GROUP BY pc.name
ORDER BY pc.name;
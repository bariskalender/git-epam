 SELECT
    p.id,
    pt.title,
    p.price
FROM product AS p
INNER JOIN product_title AS pt
    ON pt.id = p.product_title_id
ORDER BY pt.title, p.id;
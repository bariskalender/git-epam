SELECT
    p.id,
    pt.title,
    pc.name AS category,
    p.price
FROM product AS p
INNER JOIN product_title AS pt
    ON pt.id = p.product_title_id
INNER JOIN product_category AS pc
    ON pc.id = pt.product_category_id
ORDER BY pc.name, pt.title, p.id;
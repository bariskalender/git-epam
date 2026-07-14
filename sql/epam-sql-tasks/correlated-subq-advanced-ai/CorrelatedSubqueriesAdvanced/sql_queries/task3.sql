SELECT
    pt.id AS product_id,
    pt.title,
    m.id AS manufacturer_id,
    m.name AS manufacturer
FROM product_title AS pt
LEFT JOIN manufacturer AS m
    ON m.id = (
        SELECT leader.manufacturer_id
        FROM (
            SELECT
                p.manufacturer_id,
                SUM(od.product_amount) AS total_amount
            FROM product AS p
            JOIN order_details AS od
                ON od.product_id = p.id
            WHERE p.product_title_id = pt.id
            GROUP BY p.manufacturer_id
            ORDER BY total_amount DESC, p.manufacturer_id ASC
            LIMIT 1
        ) AS leader
    )
ORDER BY pt.id ASC;
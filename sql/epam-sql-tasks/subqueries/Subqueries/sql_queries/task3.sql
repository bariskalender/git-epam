SELECT
    co.id AS order_id,
    COUNT(od.id) AS items_count
FROM customer_order AS co
JOIN order_details AS od
    ON od.customer_order_id = co.id
WHERE co.operation_time >= '2021-01-01'
  AND co.operation_time < '2022-01-01'
GROUP BY co.id
HAVING COUNT(od.id) > (
    SELECT AVG(order_items_count)
    FROM (
        SELECT
            COUNT(od2.id) AS order_items_count
        FROM customer_order AS co2
        JOIN order_details AS od2
            ON od2.customer_order_id = co2.id
        GROUP BY co2.id
    ) AS order_counts
)
ORDER BY items_count ASC, order_id ASC;
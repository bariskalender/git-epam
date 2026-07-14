SELECT
    customer_order_id,
    SUM(price_with_discount * product_amount) AS to_pay
FROM order_details
GROUP BY customer_order_id
HAVING SUM(price_with_discount * product_amount) > 100
ORDER BY customer_order_id;
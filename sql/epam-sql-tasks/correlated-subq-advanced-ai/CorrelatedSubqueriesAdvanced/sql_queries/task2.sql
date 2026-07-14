SELECT
    p.id,
    p.comment AS title,
    COALESCE(
        SUM(
            CASE
                WHEN od.price < od.price_with_discount THEN 0
                WHEN ((od.price - od.price_with_discount) / od.price) > 0.05 THEN od.product_amount
                ELSE 0
            END
        ),
        0
    ) AS count_with_discount_5,
    COALESCE(
        SUM(
            CASE
                WHEN od.price < od.price_with_discount THEN 0
                WHEN ((od.price - od.price_with_discount) / od.price) <= 0.05 THEN od.product_amount
                ELSE 0
            END
        ),
        0
    ) AS count_without_discount_5,
    CASE
        WHEN COALESCE(
            SUM(
                CASE
                    WHEN od.price < od.price_with_discount THEN 0
                    WHEN ((od.price - od.price_with_discount) / od.price) <= 0.05 THEN od.product_amount
                    ELSE 0
                END
            ),
            0
        ) = 0 THEN NULL
        ELSE
            (
                COALESCE(
                    SUM(
                        CASE
                            WHEN od.price < od.price_with_discount THEN 0
                            WHEN ((od.price - od.price_with_discount) / od.price) > 0.05 THEN od.product_amount
                            ELSE 0
                        END
                    ),
                    0
                )
                - COALESCE(
                    SUM(
                        CASE
                            WHEN od.price < od.price_with_discount THEN 0
                            WHEN ((od.price - od.price_with_discount) / od.price) <= 0.05 THEN od.product_amount
                            ELSE 0
                        END
                    ),
                    0
                )
            ) * 100.0
            / COALESCE(
                SUM(
                    CASE
                        WHEN od.price < od.price_with_discount THEN 0
                        WHEN ((od.price - od.price_with_discount) / od.price) <= 0.05 THEN od.product_amount
                        ELSE 0
                    END
                ),
                0
            )
    END AS difference
FROM product AS p
LEFT JOIN order_details AS od
    ON od.product_id = p.id
GROUP BY p.id, p.comment
ORDER BY p.id ASC;
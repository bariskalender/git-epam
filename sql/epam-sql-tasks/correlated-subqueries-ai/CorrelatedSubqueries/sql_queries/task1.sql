SELECT
    c.id,
    c.name
FROM city AS c
WHERE NOT EXISTS (
    SELECT 1
    FROM street AS s
    WHERE s.city_id = c.id
)
ORDER BY c.name ASC;
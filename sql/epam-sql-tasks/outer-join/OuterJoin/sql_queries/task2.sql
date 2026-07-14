SELECT
    p.surname,
    p.name,
    p.birth_date
FROM customer AS c
INNER JOIN person AS p
    ON p.id = c.person_id
LEFT JOIN customer_order AS co
    ON co.customer_id = c.person_id
WHERE co.id IS NULL
ORDER BY p.surname, p.birth_date;
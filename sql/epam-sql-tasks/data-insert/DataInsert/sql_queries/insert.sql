INSERT INTO city (name)
VALUES
('Istanbul'),
('Ankara'),
('Izmir');

INSERT INTO street (name, city_id)
VALUES
('Ataturk Avenue', (SELECT id FROM city WHERE name = 'Istanbul')),
('Bagdat Street', (SELECT id FROM city WHERE name = 'Istanbul')),
('Tunali Hilmi', (SELECT id FROM city WHERE name = 'Ankara')),
('Kizilay Street', (SELECT id FROM city WHERE name = 'Ankara')),
('Alsancak Avenue', (SELECT id FROM city WHERE name = 'Izmir'));

INSERT INTO supermarket (name, street_id, house_number)
VALUES
('Market One', (SELECT id FROM street WHERE name = 'Ataturk Avenue'), 10),
('Market Two', (SELECT id FROM street WHERE name = 'Ataturk Avenue'), 25),
('Fresh Shop', (SELECT id FROM street WHERE name = 'Bagdat Street'), 8),
('City Market', (SELECT id FROM street WHERE name = 'Tunali Hilmi'), 15),
('Green Store', (SELECT id FROM street WHERE name = 'Kizilay Street'), 22),
('Sun Market', (SELECT id FROM street WHERE name = 'Alsancak Avenue'), 5),
('Mega Market', (SELECT id FROM street WHERE name = 'Alsancak Avenue'), 18);

INSERT INTO person (name, surname, birth_date)
VALUES
('Ahmet', 'Yilmaz', '1990-01-01'),
('Mehmet', 'Demir', '1990-01-01'),
('Ayse', 'Kaya', '1990-01-01'),
('Fatma', 'Celik', '1990-01-01'),
('Can', 'Arslan', '1990-01-01'),
('Zeynep', 'Sahin', '1990-01-01'),
('Emre', 'Aydin', '1990-01-01'),
('Elif', 'Kurt', '1990-01-01'),
('Burak', 'Tas', '1990-01-01'),
('Derya', 'Ozkan', '1990-01-01');

INSERT INTO contact_type (name)
VALUES
('Phone'),
('Email');

INSERT INTO person_contact (person_id, contact_type_id, contact_value)
VALUES
((SELECT id FROM person WHERE name = 'Ahmet' AND surname = 'Yilmaz'), (SELECT id FROM contact_type WHERE name = 'Phone'), '5551000001'),
((SELECT id FROM person WHERE name = 'Ahmet' AND surname = 'Yilmaz'), (SELECT id FROM contact_type WHERE name = 'Email'), 'ahmet@mail.com'),
((SELECT id FROM person WHERE name = 'Mehmet' AND surname = 'Demir'), (SELECT id FROM contact_type WHERE name = 'Phone'), '5551000002'),
((SELECT id FROM person WHERE name = 'Mehmet' AND surname = 'Demir'), (SELECT id FROM contact_type WHERE name = 'Email'), 'mehmet@mail.com'),
((SELECT id FROM person WHERE name = 'Ayse' AND surname = 'Kaya'), (SELECT id FROM contact_type WHERE name = 'Phone'), '5551000003'),
((SELECT id FROM person WHERE name = 'Ayse' AND surname = 'Kaya'), (SELECT id FROM contact_type WHERE name = 'Email'), 'ayse@mail.com'),
((SELECT id FROM person WHERE name = 'Fatma' AND surname = 'Celik'), (SELECT id FROM contact_type WHERE name = 'Phone'), '5551000004'),
((SELECT id FROM person WHERE name = 'Fatma' AND surname = 'Celik'), (SELECT id FROM contact_type WHERE name = 'Email'), 'fatma@mail.com'),
((SELECT id FROM person WHERE name = 'Can' AND surname = 'Arslan'), (SELECT id FROM contact_type WHERE name = 'Phone'), '5551000005'),
((SELECT id FROM person WHERE name = 'Can' AND surname = 'Arslan'), (SELECT id FROM contact_type WHERE name = 'Email'), 'can@mail.com'),
((SELECT id FROM person WHERE name = 'Zeynep' AND surname = 'Sahin'), (SELECT id FROM contact_type WHERE name = 'Phone'), '5551000006'),
((SELECT id FROM person WHERE name = 'Zeynep' AND surname = 'Sahin'), (SELECT id FROM contact_type WHERE name = 'Email'), 'zeynep@mail.com'),
((SELECT id FROM person WHERE name = 'Emre' AND surname = 'Aydin'), (SELECT id FROM contact_type WHERE name = 'Phone'), '5551000007'),
((SELECT id FROM person WHERE name = 'Emre' AND surname = 'Aydin'), (SELECT id FROM contact_type WHERE name = 'Email'), 'emre@mail.com'),
((SELECT id FROM person WHERE name = 'Elif' AND surname = 'Kurt'), (SELECT id FROM contact_type WHERE name = 'Phone'), '5551000008'),
((SELECT id FROM person WHERE name = 'Elif' AND surname = 'Kurt'), (SELECT id FROM contact_type WHERE name = 'Email'), 'elif@mail.com'),
((SELECT id FROM person WHERE name = 'Burak' AND surname = 'Tas'), (SELECT id FROM contact_type WHERE name = 'Phone'), '5551000009'),
((SELECT id FROM person WHERE name = 'Burak' AND surname = 'Tas'), (SELECT id FROM contact_type WHERE name = 'Email'), 'burak@mail.com'),
((SELECT id FROM person WHERE name = 'Derya' AND surname = 'Ozkan'), (SELECT id FROM contact_type WHERE name = 'Phone'), '5551000010'),
((SELECT id FROM person WHERE name = 'Derya' AND surname = 'Ozkan'), (SELECT id FROM contact_type WHERE name = 'Email'), 'derya@mail.com');

INSERT INTO customer (person_id, card_number, discount)
VALUES
((SELECT id FROM person WHERE name = 'Ahmet' AND surname = 'Yilmaz'), 'CARD1001', 1),
((SELECT id FROM person WHERE name = 'Mehmet' AND surname = 'Demir'), 'CARD1002', 2),
((SELECT id FROM person WHERE name = 'Ayse' AND surname = 'Kaya'), 'CARD1003', 3),
((SELECT id FROM person WHERE name = 'Fatma' AND surname = 'Celik'), 'CARD1004', 4),
((SELECT id FROM person WHERE name = 'Can' AND surname = 'Arslan'), 'CARD1005', 5),
((SELECT id FROM person WHERE name = 'Zeynep' AND surname = 'Sahin'), 'CARD1006', 6),
((SELECT id FROM person WHERE name = 'Emre' AND surname = 'Aydin'), 'CARD1007', 7),
((SELECT id FROM person WHERE name = 'Elif' AND surname = 'Kurt'), 'CARD1008', 8),
((SELECT id FROM person WHERE name = 'Burak' AND surname = 'Tas'), 'CARD1009', 9),
((SELECT id FROM person WHERE name = 'Derya' AND surname = 'Ozkan'), 'CARD1010', 10);

INSERT INTO product_category (name)
VALUES
('Beverages'),
('Bakery'),
('Dairy');

INSERT INTO product_title (title, product_category_id)
VALUES
('Cola', (SELECT id FROM product_category WHERE name = 'Beverages')),
('Orange Juice', (SELECT id FROM product_category WHERE name = 'Beverages')),
('Water', (SELECT id FROM product_category WHERE name = 'Beverages')),
('Tea', (SELECT id FROM product_category WHERE name = 'Beverages')),
('Bread', (SELECT id FROM product_category WHERE name = 'Bakery')),
('Croissant', (SELECT id FROM product_category WHERE name = 'Bakery')),
('Bagel', (SELECT id FROM product_category WHERE name = 'Bakery')),
('Milk', (SELECT id FROM product_category WHERE name = 'Dairy')),
('Cheese', (SELECT id FROM product_category WHERE name = 'Dairy')),
('Yogurt', (SELECT id FROM product_category WHERE name = 'Dairy'));

INSERT INTO manufacturer (name)
VALUES
('Ulker'),
('Pinar'),
('Eti'),
('Sek');

INSERT INTO product (product_title_id, manufacturer_id, price, comment)
VALUES
((SELECT id FROM product_title WHERE title = 'Cola'), (SELECT id FROM manufacturer WHERE name = 'Ulker'), 25.00, 'Cola product 1'),
((SELECT id FROM product_title WHERE title = 'Cola'), (SELECT id FROM manufacturer WHERE name = 'Eti'), 27.00, 'Cola product 2'),
((SELECT id FROM product_title WHERE title = 'Orange Juice'), (SELECT id FROM manufacturer WHERE name = 'Pinar'), 30.00, 'Orange juice product 1'),
((SELECT id FROM product_title WHERE title = 'Orange Juice'), (SELECT id FROM manufacturer WHERE name = 'Sek'), 32.00, 'Orange juice product 2'),
((SELECT id FROM product_title WHERE title = 'Water'), (SELECT id FROM manufacturer WHERE name = 'Ulker'), 10.00, 'Water product 1'),
((SELECT id FROM product_title WHERE title = 'Water'), (SELECT id FROM manufacturer WHERE name = 'Pinar'), 11.00, 'Water product 2'),
((SELECT id FROM product_title WHERE title = 'Tea'), (SELECT id FROM manufacturer WHERE name = 'Eti'), 35.00, 'Tea product 1'),
((SELECT id FROM product_title WHERE title = 'Tea'), (SELECT id FROM manufacturer WHERE name = 'Ulker'), 36.00, 'Tea product 2'),
((SELECT id FROM product_title WHERE title = 'Bread'), (SELECT id FROM manufacturer WHERE name = 'Eti'), 15.00, 'Bread product 1'),
((SELECT id FROM product_title WHERE title = 'Bread'), (SELECT id FROM manufacturer WHERE name = 'Ulker'), 16.00, 'Bread product 2'),
((SELECT id FROM product_title WHERE title = 'Croissant'), (SELECT id FROM manufacturer WHERE name = 'Eti'), 18.00, 'Croissant product 1'),
((SELECT id FROM product_title WHERE title = 'Croissant'), (SELECT id FROM manufacturer WHERE name = 'Pinar'), 19.00, 'Croissant product 2'),
((SELECT id FROM product_title WHERE title = 'Bagel'), (SELECT id FROM manufacturer WHERE name = 'Ulker'), 14.00, 'Bagel product 1'),
((SELECT id FROM product_title WHERE title = 'Bagel'), (SELECT id FROM manufacturer WHERE name = 'Sek'), 15.00, 'Bagel product 2'),
((SELECT id FROM product_title WHERE title = 'Milk'), (SELECT id FROM manufacturer WHERE name = 'Pinar'), 22.00, 'Milk product 1'),
((SELECT id FROM product_title WHERE title = 'Milk'), (SELECT id FROM manufacturer WHERE name = 'Sek'), 23.00, 'Milk product 2'),
((SELECT id FROM product_title WHERE title = 'Cheese'), (SELECT id FROM manufacturer WHERE name = 'Sek'), 40.00, 'Cheese product 1'),
((SELECT id FROM product_title WHERE title = 'Cheese'), (SELECT id FROM manufacturer WHERE name = 'Pinar'), 42.00, 'Cheese product 2'),
((SELECT id FROM product_title WHERE title = 'Yogurt'), (SELECT id FROM manufacturer WHERE name = 'Sek'), 20.00, 'Yogurt product 1'),
((SELECT id FROM product_title WHERE title = 'Yogurt'), (SELECT id FROM manufacturer WHERE name = 'Ulker'), 21.00, 'Yogurt product 2');

INSERT INTO customer_order (operation_time, supermarket_id, customer_id)
VALUES
('2025-01-01', (SELECT id FROM supermarket WHERE name = 'Market One'), (SELECT person_id FROM customer WHERE card_number = 'CARD1001')),
('2025-01-02', (SELECT id FROM supermarket WHERE name = 'Market Two'), (SELECT person_id FROM customer WHERE card_number = 'CARD1002')),
('2025-01-03', (SELECT id FROM supermarket WHERE name = 'Fresh Shop'), (SELECT person_id FROM customer WHERE card_number = 'CARD1003')),
('2025-01-04', (SELECT id FROM supermarket WHERE name = 'City Market'), (SELECT person_id FROM customer WHERE card_number = 'CARD1004')),
('2025-01-05', (SELECT id FROM supermarket WHERE name = 'Green Store'), (SELECT person_id FROM customer WHERE card_number = 'CARD1005')),
('2025-01-06', (SELECT id FROM supermarket WHERE name = 'Sun Market'), (SELECT person_id FROM customer WHERE card_number = 'CARD1006')),
('2025-01-07', (SELECT id FROM supermarket WHERE name = 'Mega Market'), (SELECT person_id FROM customer WHERE card_number = 'CARD1007')),
('2025-01-08', (SELECT id FROM supermarket WHERE name = 'Market One'), (SELECT person_id FROM customer WHERE card_number = 'CARD1008')),
('2025-01-09', (SELECT id FROM supermarket WHERE name = 'Market Two'), (SELECT person_id FROM customer WHERE card_number = 'CARD1009')),
('2025-01-10', (SELECT id FROM supermarket WHERE name = 'Fresh Shop'), (SELECT person_id FROM customer WHERE card_number = 'CARD1010')),
('2025-01-11', (SELECT id FROM supermarket WHERE name = 'City Market'), (SELECT person_id FROM customer WHERE card_number = 'CARD1001')),
('2025-01-12', (SELECT id FROM supermarket WHERE name = 'Green Store'), (SELECT person_id FROM customer WHERE card_number = 'CARD1002')),
('2025-01-13', (SELECT id FROM supermarket WHERE name = 'Sun Market'), (SELECT person_id FROM customer WHERE card_number = 'CARD1003')),
('2025-01-14', (SELECT id FROM supermarket WHERE name = 'Mega Market'), (SELECT person_id FROM customer WHERE card_number = 'CARD1004')),
('2025-01-15', (SELECT id FROM supermarket WHERE name = 'Market One'), (SELECT person_id FROM customer WHERE card_number = 'CARD1005')),
('2025-01-16', (SELECT id FROM supermarket WHERE name = 'Market Two'), (SELECT person_id FROM customer WHERE card_number = 'CARD1006')),
('2025-01-17', (SELECT id FROM supermarket WHERE name = 'Fresh Shop'), (SELECT person_id FROM customer WHERE card_number = 'CARD1007')),
('2025-01-18', (SELECT id FROM supermarket WHERE name = 'City Market'), (SELECT person_id FROM customer WHERE card_number = 'CARD1008')),
('2025-01-19', (SELECT id FROM supermarket WHERE name = 'Green Store'), (SELECT person_id FROM customer WHERE card_number = 'CARD1009')),
('2025-01-20', (SELECT id FROM supermarket WHERE name = 'Sun Market'), (SELECT person_id FROM customer WHERE card_number = 'CARD1010'));

INSERT INTO order_details (customer_order_id, product_id, price, price_with_discount, product_amount)
VALUES
((SELECT id FROM customer_order WHERE operation_time = '2025-01-01'), (SELECT id FROM product WHERE comment = 'Cola product 1'), 25.00, 24.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-02'), (SELECT id FROM product WHERE comment = 'Cola product 2'), 27.00, 26.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-03'), (SELECT id FROM product WHERE comment = 'Orange juice product 1'), 30.00, 29.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-04'), (SELECT id FROM product WHERE comment = 'Orange juice product 2'), 32.00, 31.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-05'), (SELECT id FROM product WHERE comment = 'Water product 1'), 10.00, 9.50, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-06'), (SELECT id FROM product WHERE comment = 'Water product 2'), 11.00, 10.50, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-07'), (SELECT id FROM product WHERE comment = 'Tea product 1'), 35.00, 34.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-08'), (SELECT id FROM product WHERE comment = 'Tea product 2'), 36.00, 35.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-09'), (SELECT id FROM product WHERE comment = 'Bread product 1'), 15.00, 14.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-10'), (SELECT id FROM product WHERE comment = 'Bread product 2'), 16.00, 15.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-11'), (SELECT id FROM product WHERE comment = 'Croissant product 1'), 18.00, 17.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-12'), (SELECT id FROM product WHERE comment = 'Croissant product 2'), 19.00, 18.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-13'), (SELECT id FROM product WHERE comment = 'Bagel product 1'), 14.00, 13.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-14'), (SELECT id FROM product WHERE comment = 'Bagel product 2'), 15.00, 14.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-15'), (SELECT id FROM product WHERE comment = 'Milk product 1'), 22.00, 21.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-16'), (SELECT id FROM product WHERE comment = 'Milk product 2'), 23.00, 22.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-17'), (SELECT id FROM product WHERE comment = 'Cheese product 1'), 40.00, 39.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-18'), (SELECT id FROM product WHERE comment = 'Cheese product 2'), 42.00, 41.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-19'), (SELECT id FROM product WHERE comment = 'Yogurt product 1'), 20.00, 19.00, 1),
((SELECT id FROM customer_order WHERE operation_time = '2025-01-20'), (SELECT id FROM product WHERE comment = 'Yogurt product 2'), 21.00, 20.00, 1);
CREATE TABLE `item` (
  `id` int(11) NOT NULL,
  `code` varchar(5) NOT NULL,
  `description` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `itemcode_UNIQUE` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


DELIMITER $$
CREATE DEFINER=`mysqluser`@`%` PROCEDURE `GetItems`(
IN pItemId INT
)
BEGIN
SELECT *
FROM item i
WHERE pItemId IS NULL OR pItemId = i.Id;
END$$
DELIMITER ;

SELECT *
FROM HomeworkDb.dbo.[User]
LEFT JOIN UserRole ON HomeworkDb.dbo.[User].Id = UserRole.UserId
LEFT JOIN Role ON HomeworkDb.dbo.[UserRole].RoleId = Role.Id
WHERE Role.Name LIKE 'Administrator%';
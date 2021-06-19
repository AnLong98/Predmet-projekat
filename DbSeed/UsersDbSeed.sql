USE [UsersDatabase]
GO

GO
SET IDENTITY_INSERT [dbo].[Crews] ON 

INSERT [dbo].[Crews] ([ID], [CrewName]) VALUES (2, N'Ekipa2')
INSERT [dbo].[Crews] ([ID], [CrewName]) VALUES (3, N'Ekipica')
SET IDENTITY_INSERT [dbo].[Crews] OFF
GO

SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (1, N'nidza98', N'nekibezvezni@gmail.com', N'Nikola', N'Mijonic', N'$2a$04$16Pf3sY7eXgeVZGoRmRbAOe4RsIxut.XCwT9vbIjtQJrcQShytfRW', CAST(N'1998-06-03T00:00:00.0000000' AS DateTime2), N'ADMIN', NULL, NULL, 2, N'APPROVED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (4, N'sveto98', N'svetoizvorac@yahoo.com', N'Svetozar', N'Izvoric', N'$2a$04$16Pf3sY7eXgeVZGoRmRbAOe4RsIxut.XCwT9vbIjtQJrcQShytfRW', CAST(N'1998-05-03T00:00:00.0000000' AS DateTime2), N'ADMIN', NULL, NULL, 7, N'APPROVED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (5, N'stele98', N'vuci@gmail.com', N'Stefan', N'Besovic', N'$2a$11$L.fb./NAUzUTNLGFJiv8quleGSjDb.30RCG2BKYjxp6GNtGIT5/ji', CAST(N'1999-06-08T00:00:00.0000000' AS DateTime2), N'DISPATCHER', N'5024f4ae-1709-49d6-9b8c-263b6dba6b7d.jpg', NULL, 4, N'APPROVED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (6, N'rokajlo', N'roki98@gmail.com', N'Vladimir', N'Rodusek', N'$2a$11$1rVTVYnxDjlBnX.sbgQA0eSAwJOHT0tCQumkXtkGWVfuGYmGXWhKy', CAST(N'1993-05-03T00:00:00.0000000' AS DateTime2), N'WORKER', N'pedja-grades.JPG', NULL, 1, N'DENIED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (7, N'badrlja', N'badrlja@gmail.com', N'DRagisa', N'Badrljica', N'$2a$11$Mx/mrBwRgNWCDRvATfWxu.4gv4lTye19IrmVavhUveQhBRVFc90wi', CAST(N'1996-08-09T00:00:00.0000000' AS DateTime2), N'CONSUMER', NULL, NULL, 2, N'APPROVED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (8, N'p.glavas', N'p.glavas@yandex.com', N'Предраг', N'Главаш', N'2857725861144145', CAST(N'2021-06-06T23:19:34.9514760' AS DateTime2), N'CONSUMER', NULL, NULL, 1, N'APPROVED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (10, N'crew1', N'ekipar@hotmail.com', N'Ekipar', N'Prvi', N'$2a$04$16Pf3sY7eXgeVZGoRmRbAOe4RsIxut.XCwT9vbIjtQJrcQShytfRW', CAST(N'1996-05-03T00:00:00.0000000' AS DateTime2), N'CREW_MEMBER', NULL, NULL, 3, N'APPROVED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (14, N'crew2', N'ekipar2@gmaul.com', N'Ekipar', N'Drugi', N'$2a$04$16Pf3sY7eXgeVZGoRmRbAOe4RsIxut.XCwT9vbIjtQJrcQShytfRW', CAST(N'1970-02-11T00:00:00.0000000' AS DateTime2), N'CREW_MEMBER', NULL, 2, 6, N'APPROVED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (15, N'CRE4', N'ekipar4@hotmail.com', N'Ekipar', N'Cetvrti', N'$2a$04$16Pf3sY7eXgeVZGoRmRbAOe4RsIxut.XCwT9vbIjtQJrcQShytfRW', CAST(N'1987-03-24T00:00:00.0000000' AS DateTime2), N'CREW_MEMBER', NULL, 3, 2, N'APPROVED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (16, N'crew3', N'ekipa@gmail.com', N'EKIPAR', N'PETI', N'$2a$04$16Pf3sY7eXgeVZGoRmRbAOe4RsIxut.XCwT9vbIjtQJrcQShytfRW', CAST(N'2001-01-31T00:00:00.0000000' AS DateTime2), N'CREW_MEMBER', NULL, 3, 3, N'APPROVED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (17, N'smart.energy.pusgs', N'smart.energy.pusgs@gmail.com', N'PUSGS', N'2021', N'55f458b6726fde658ad7b01b3112cdca67d7e2cd', CAST(N'2021-06-07T00:09:35.4654987' AS DateTime2), N'DISPATCHER', NULL, NULL, 1, N'APPROVED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (20, N'KONZUMENT', N'nikolamijonic@gmail.com', N'Kunz', N'Umnet', N'$2a$04$16Pf3sY7eXgeVZGoRmRbAOe4RsIxut.XCwT9vbIjtQJrcQShytfRW', CAST(N'1990-08-05T00:00:00.0000000' AS DateTime2), N'CONSUMER', NULL, NULL, 4, N'APPROVED')
INSERT [dbo].[Users] ([ID], [Username], [Email], [Name], [Lastname], [Password], [BirthDay], [UserType], [ImageURL], [CrewID], [LocationID], [UserStatus]) VALUES (21, N'pedjaglavas98', N'pedjaglavas98@gmail.com', N'Predrag', N'Glavas', N'b5b9351f0c85c299eb5d90f869ec3c50480ebac1', CAST(N'2021-06-07T21:01:11.3258879' AS DateTime2), N'CONSUMER', NULL, NULL, 1, N'PENDING')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO

SET IDENTITY_INSERT [dbo].[Consumers] ON 

INSERT [dbo].[Consumers] ([ID], [Name], [Lastname], [Phone], [AccountID], [AccountType], [UserID], [LocationID]) VALUES (1, N'Konzumer', N'Neki', N'022462484', N'1223213', 0, NULL, 1)
INSERT [dbo].[Consumers] ([ID], [Name], [Lastname], [Phone], [AccountID], [AccountType], [UserID], [LocationID]) VALUES (3, N'Petar', N'Petrovic', N'021454895', N'3454345', 1, NULL, 3)
INSERT [dbo].[Consumers] ([ID], [Name], [Lastname], [Phone], [AccountID], [AccountType], [UserID], [LocationID]) VALUES (5, N'Jovica', N'Bulut', N'324234234', N'4564255', 0, 20, 4)
INSERT [dbo].[Consumers] ([ID], [Name], [Lastname], [Phone], [AccountID], [AccountType], [UserID], [LocationID]) VALUES (6, N'Zdravko', N'Rogan', N'402342904', N'2349023', 0, 8, 4)
INSERT [dbo].[Consumers] ([ID], [Name], [Lastname], [Phone], [AccountID], [AccountType], [UserID], [LocationID]) VALUES (7, N'Nadja', N'Gutova', N'324324234', N'2343244', 1, NULL, 2)
SET IDENTITY_INSERT [dbo].[Consumers] OFF
GO
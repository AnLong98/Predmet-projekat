USE [PhysicalDatabase]

SET IDENTITY_INSERT [dbo].[Location] ON 

INSERT [dbo].[Location] ([ID], [Street], [City], [Zip], [MorningPriority], [NoonPriority], [NightPriority], [Latitude], [Longitude], [Number]) VALUES (1, N'Vase Jagazovica', N'Veternik', N'21000', 1, 2, 2, 45.245054739266195, 19.779118277456949, 30)
INSERT [dbo].[Location] ([ID], [Street], [City], [Zip], [MorningPriority], [NoonPriority], [NightPriority], [Latitude], [Longitude], [Number]) VALUES (2, N'Bulevar Evrope', N'Novi Sad', N'21000', 2, 3, 4, 45.24489466200648, 19.819271061114858, 28)
INSERT [dbo].[Location] ([ID], [Street], [City], [Zip], [MorningPriority], [NoonPriority], [NightPriority], [Latitude], [Longitude], [Number]) VALUES (3, N'Bulevar Cara Lazara', N'Novi Sad', N'21000', 3, 1, 3, 45.244324702807631, 19.843701390634706, 19)
INSERT [dbo].[Location] ([ID], [Street], [City], [Zip], [MorningPriority], [NoonPriority], [NightPriority], [Latitude], [Longitude], [Number]) VALUES (4, N'Vojvode Stepe ', N'Indjija', N'22320', 3, 1, 1, 45.048539007282038, 20.081929176984737, 6)
INSERT [dbo].[Location] ([ID], [Street], [City], [Zip], [MorningPriority], [NoonPriority], [NightPriority], [Latitude], [Longitude], [Number]) VALUES (6, N'Nikole Bursaca', N'Indjija', N'22320', 1, 1, 2, 45.040740653738048, 20.094191843793059, 37)
INSERT [dbo].[Location] ([ID], [Street], [City], [Zip], [MorningPriority], [NoonPriority], [NightPriority], [Latitude], [Longitude], [Number]) VALUES (7, N'Maksima Gorkog', N'Beograd', N'11000', 3, 3, 3, 44.797304828084727, 20.477165749765437, 39)
INSERT [dbo].[Location] ([ID], [Street], [City], [Zip], [MorningPriority], [NoonPriority], [NightPriority], [Latitude], [Longitude], [Number]) VALUES (8, N'Mikenska', N'Beograd', N'11000', 3, 4, 3, 44.777006844405669, 20.515251937738249, 24)
SET IDENTITY_INSERT [dbo].[Location] OFF
GO

SET IDENTITY_INSERT [dbo].[Devices] ON 

INSERT [dbo].[Devices] ([ID], [DeviceType], [Name], [LocationID], [DeviceCounter], [Timestamp]) VALUES (1, N'DISCONNECTOR', N'DIS1', 1, 1, CAST(N'2021-06-06T23:42:45.2759950' AS DateTime2))
INSERT [dbo].[Devices] ([ID], [DeviceType], [Name], [LocationID], [DeviceCounter], [Timestamp]) VALUES (2, N'FUSE', N'FUS2', 4, 2, CAST(N'2021-06-06T23:42:07.4420726' AS DateTime2))
INSERT [dbo].[Devices] ([ID], [DeviceType], [Name], [LocationID], [DeviceCounter], [Timestamp]) VALUES (4, N'POWER_SWITCH', N'POW4', 7, 4, CAST(N'2021-06-06T23:42:22.7247712' AS DateTime2))
INSERT [dbo].[Devices] ([ID], [DeviceType], [Name], [LocationID], [DeviceCounter], [Timestamp]) VALUES (5, N'FUSE', N'FUS5', 6, 5, CAST(N'2021-06-06T23:42:30.9578908' AS DateTime2))
INSERT [dbo].[Devices] ([ID], [DeviceType], [Name], [LocationID], [DeviceCounter], [Timestamp]) VALUES (6, N'TRANSFORMER', N'TRA6', 4, 6, CAST(N'2021-06-06T23:42:38.4842247' AS DateTime2))
INSERT [dbo].[Devices] ([ID], [DeviceType], [Name], [LocationID], [DeviceCounter], [Timestamp]) VALUES (7, N'DISCONNECTOR', N'DIS7', 6, 7, CAST(N'2021-06-07T11:19:16.2521278' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Devices] OFF
GO

SET IDENTITY_INSERT [dbo].[DeviceUsages] ON 

INSERT [dbo].[DeviceUsages] ([ID], [IncidentID], [WorkRequestID], [WorkPlanID], [SafetyDocumentID], [DeviceID]) VALUES (1, 1, 1, 2, 2, 2)
INSERT [dbo].[DeviceUsages] ([ID], [IncidentID], [WorkRequestID], [WorkPlanID], [SafetyDocumentID], [DeviceID]) VALUES (3, 1, 1, 2, 2, 6)
INSERT [dbo].[DeviceUsages] ([ID], [IncidentID], [WorkRequestID], [WorkPlanID], [SafetyDocumentID], [DeviceID]) VALUES (4, 3, 2, 3, NULL, 1)
INSERT [dbo].[DeviceUsages] ([ID], [IncidentID], [WorkRequestID], [WorkPlanID], [SafetyDocumentID], [DeviceID]) VALUES (5, 2, 3, NULL, NULL, 4)
SET IDENTITY_INSERT [dbo].[DeviceUsages] OFF
GO
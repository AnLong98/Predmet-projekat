USE [DocumentsDatabase]
GO

SET IDENTITY_INSERT [dbo].[MultimediaAnchors] ON 

INSERT [dbo].[MultimediaAnchors] ([ID]) VALUES (1)
INSERT [dbo].[MultimediaAnchors] ([ID]) VALUES (2)
INSERT [dbo].[MultimediaAnchors] ([ID]) VALUES (3)
INSERT [dbo].[MultimediaAnchors] ([ID]) VALUES (4)
INSERT [dbo].[MultimediaAnchors] ([ID]) VALUES (5)
INSERT [dbo].[MultimediaAnchors] ([ID]) VALUES (6)
INSERT [dbo].[MultimediaAnchors] ([ID]) VALUES (7)
INSERT [dbo].[MultimediaAnchors] ([ID]) VALUES (8)
INSERT [dbo].[MultimediaAnchors] ([ID]) VALUES (9)
SET IDENTITY_INSERT [dbo].[MultimediaAnchors] OFF
GO
SET IDENTITY_INSERT [dbo].[NotificationAnchors] ON 

INSERT [dbo].[NotificationAnchors] ([ID]) VALUES (1)
INSERT [dbo].[NotificationAnchors] ([ID]) VALUES (2)
INSERT [dbo].[NotificationAnchors] ([ID]) VALUES (3)
INSERT [dbo].[NotificationAnchors] ([ID]) VALUES (4)
INSERT [dbo].[NotificationAnchors] ([ID]) VALUES (5)
INSERT [dbo].[NotificationAnchors] ([ID]) VALUES (6)
INSERT [dbo].[NotificationAnchors] ([ID]) VALUES (7)
INSERT [dbo].[NotificationAnchors] ([ID]) VALUES (8)
INSERT [dbo].[NotificationAnchors] ([ID]) VALUES (9)
SET IDENTITY_INSERT [dbo].[NotificationAnchors] OFF
GO
SET IDENTITY_INSERT [dbo].[Incidents] ON 

INSERT [dbo].[Incidents] ([ID], [Priority], [Confirmed], [ETA], [ATA], [ETR], [IncidentDateTime], [WorkBeginDate], [VoltageLevel], [UserID], [CrewID], [MultimediaAnchorID], [NotificationAnchorID], [WorkType], [IncidentStatus], [Description], [Timestamp]) VALUES (1, 1, 0, CAST(N'2021-06-15T10:29:00.0000000' AS DateTime2), CAST(N'2021-06-17T11:06:00.0000000' AS DateTime2), NULL, CAST(N'2021-06-01T19:26:00.0000000' AS DateTime2), CAST(N'2021-06-08T10:32:00.0000000' AS DateTime2), 220, 14, NULL, 1, 1, N'PLANNED', N'UNRESOLVED', N'', CAST(N'2021-06-06T23:28:30.2937366' AS DateTime2))
INSERT [dbo].[Incidents] ([ID], [Priority], [Confirmed], [ETA], [ATA], [ETR], [IncidentDateTime], [WorkBeginDate], [VoltageLevel], [UserID], [CrewID], [MultimediaAnchorID], [NotificationAnchorID], [WorkType], [IncidentStatus], [Description], [Timestamp]) VALUES (2, 3, 1, CAST(N'2021-07-29T11:33:00.0000000' AS DateTime2), CAST(N'2021-08-28T11:34:00.0000000' AS DateTime2), NULL, CAST(N'2021-06-15T12:34:00.0000000' AS DateTime2), CAST(N'2021-06-28T01:34:00.0000000' AS DateTime2), 380, 5, NULL, 2, 2, N'UNPLANNED', N'UNRESOLVED', N'', CAST(N'2021-06-06T23:29:45.6040200' AS DateTime2))
INSERT [dbo].[Incidents] ([ID], [Priority], [Confirmed], [ETA], [ATA], [ETR], [IncidentDateTime], [WorkBeginDate], [VoltageLevel], [UserID], [CrewID], [MultimediaAnchorID], [NotificationAnchorID], [WorkType], [IncidentStatus], [Description], [Timestamp]) VALUES (3, 2, 1, CAST(N'2021-06-29T12:34:00.0000000' AS DateTime2), CAST(N'2021-06-30T01:35:00.0000000' AS DateTime2), CAST(N'2021-06-06T07:31:04.0260000' AS DateTime2), CAST(N'2021-02-16T00:31:00.0000000' AS DateTime2), CAST(N'2021-04-14T23:32:00.0000000' AS DateTime2), 500, 5, NULL, 3, 3, N'UNPLANNED', N'UNRESOLVED', N'opis neki', CAST(N'2021-06-06T23:31:04.1956688' AS DateTime2))
INSERT [dbo].[Incidents] ([ID], [Priority], [Confirmed], [ETA], [ATA], [ETR], [IncidentDateTime], [WorkBeginDate], [VoltageLevel], [UserID], [CrewID], [MultimediaAnchorID], [NotificationAnchorID], [WorkType], [IncidentStatus], [Description], [Timestamp]) VALUES (4, 0, 0, CAST(N'2021-06-23T00:34:00.0000000' AS DateTime2), CAST(N'2021-08-27T00:33:00.0000000' AS DateTime2), CAST(N'2021-06-06T02:34:00.5970000' AS DateTime2), CAST(N'2021-06-17T23:34:00.0000000' AS DateTime2), CAST(N'2021-06-25T12:34:00.0000000' AS DateTime2), 30, 5, NULL, 4, 4, N'UNPLANNED', N'INITIAL', N'', CAST(N'2021-06-06T23:32:00.7544271' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Incidents] OFF
GO

SET IDENTITY_INSERT [dbo].[Calls] ON 

INSERT [dbo].[Calls] ([ID], [CallReason], [Comment], [Hazard], [LocationID], [ConsumerID], [IncidentID]) VALUES (1, N'NO_POWER', N'komeatrcic', N'safjdofjs', 4, NULL, 1)
INSERT [dbo].[Calls] ([ID], [CallReason], [Comment], [Hazard], [LocationID], [ConsumerID], [IncidentID]) VALUES (2, N'FLICKERING_LIGHT', N'SADSD', N'ADSADA', 4, 5, 1)
INSERT [dbo].[Calls] ([ID], [CallReason], [Comment], [Hazard], [LocationID], [ConsumerID], [IncidentID]) VALUES (3, N'VOLTAGE_PROBLEM', N'SFDF', N'FDSF', 4, 1, 1)
INSERT [dbo].[Calls] ([ID], [CallReason], [Comment], [Hazard], [LocationID], [ConsumerID], [IncidentID]) VALUES (5, N'POWER_RESTORED', N'DSFDSF', N'SFDFDS', 3, 1, NULL)
INSERT [dbo].[Calls] ([ID], [CallReason], [Comment], [Hazard], [LocationID], [ConsumerID], [IncidentID]) VALUES (6, N'MALFUNCTION', N'komentiram nestoo', N'Lupam nesto batee', 6, NULL, 1)
INSERT [dbo].[Calls] ([ID], [CallReason], [Comment], [Hazard], [LocationID], [ConsumerID], [IncidentID]) VALUES (7, N'FLICKERING_LIGHT', N'', N'neki hazard bakisa', 4, 6, 1)
SET IDENTITY_INSERT [dbo].[Calls] OFF
GO
SET IDENTITY_INSERT [dbo].[MultimediaAttachments] ON 

INSERT [dbo].[MultimediaAttachments] ([ID], [Url], [MultimediaAnchorID]) VALUES (5, N'cv-glavas.pdf', 5)
INSERT [dbo].[MultimediaAttachments] ([ID], [Url], [MultimediaAnchorID]) VALUES (6, N'5024f4ae-1709-49d6-9b8c-263b6dba6b7d.jpg', 7)
INSERT [dbo].[MultimediaAttachments] ([ID], [Url], [MultimediaAnchorID]) VALUES (7, N'device.png', 5)
SET IDENTITY_INSERT [dbo].[MultimediaAttachments] OFF
GO
SET IDENTITY_INSERT [dbo].[StateChangeAnchors] ON 

INSERT [dbo].[StateChangeAnchors] ([ID]) VALUES (1)
INSERT [dbo].[StateChangeAnchors] ([ID]) VALUES (2)
INSERT [dbo].[StateChangeAnchors] ([ID]) VALUES (3)
INSERT [dbo].[StateChangeAnchors] ([ID]) VALUES (4)
INSERT [dbo].[StateChangeAnchors] ([ID]) VALUES (5)
SET IDENTITY_INSERT [dbo].[StateChangeAnchors] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkRequests] ON 

INSERT [dbo].[WorkRequests] ([ID], [StartDate], [EndDate], [CreatedOn], [Purpose], [Note], [CompanyName], [Phone], [Street], [DocumentType], [DocumentStatus], [IsEmergency], [UserID], [IncidentID], [MultimediaAnchorID], [StateChangeAnchorID], [NotificationAnchorID], [Details]) VALUES (1, CAST(N'2021-06-15T20:00:00.0000000' AS DateTime2), CAST(N'2021-06-29T20:00:00.0000000' AS DateTime2), CAST(N'2021-06-06T22:04:37.6920000' AS DateTime2), N'razlog neki nesot tako', N'', N'', N'3092432904', N'Vojvode Stepe , Indjija', N'UNPLANNED', N'DRAFT', 0, 5, 1, 5, 1, 5, N'')
INSERT [dbo].[WorkRequests] ([ID], [StartDate], [EndDate], [CreatedOn], [Purpose], [Note], [CompanyName], [Phone], [Street], [DocumentType], [DocumentStatus], [IsEmergency], [UserID], [IncidentID], [MultimediaAnchorID], [StateChangeAnchorID], [NotificationAnchorID], [Details]) VALUES (2, CAST(N'2021-06-14T22:00:00.0000000' AS DateTime2), CAST(N'2021-06-15T22:00:00.0000000' AS DateTime2), CAST(N'2021-06-06T22:08:07.7420000' AS DateTime2), N'sssdsd', N'', N'', N'', N'Vase Jagazovica, Veternik', N'PLANNED', N'DRAFT', 0, 5, 3, 6, 2, 6, N'')
INSERT [dbo].[WorkRequests] ([ID], [StartDate], [EndDate], [CreatedOn], [Purpose], [Note], [CompanyName], [Phone], [Street], [DocumentType], [DocumentStatus], [IsEmergency], [UserID], [IncidentID], [MultimediaAnchorID], [StateChangeAnchorID], [NotificationAnchorID], [Details]) VALUES (3, CAST(N'2021-06-15T22:00:00.0000000' AS DateTime2), CAST(N'2021-06-24T22:00:00.0000000' AS DateTime2), CAST(N'2021-06-07T09:15:12.1560000' AS DateTime2), N'SDASDAS', N'', N'', N'', N'Maksima Gorkog, Beograd', N'UNPLANNED', N'DRAFT', 0, 5, 2, 9, 5, 9, N'')
SET IDENTITY_INSERT [dbo].[WorkRequests] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkPlans] ON 

INSERT [dbo].[WorkPlans] ([ID], [Purpose], [Notes], [Phone], [Street], [CompanyName], [CreatedOn], [StartDate], [EndDate], [DocumentType], [DocumentStatus], [UserID], [MultimediaAnchorID], [StateChangeAnchorID], [NotificationAnchorID], [WorkRequestID]) VALUES (2, N'DFDSF', N'Neka beleska', N'021984393', N'Vasa Jagzovica', N'ddsd', CAST(N'2021-06-07T00:13:19.4700000' AS DateTime2), CAST(N'2021-06-20T00:00:00.0000000' AS DateTime2), CAST(N'2021-06-25T00:00:00.0000000' AS DateTime2), N'APPROVED', N'PLANNED', 5, NULL, NULL, NULL, 1)
INSERT [dbo].[WorkPlans] ([ID], [Purpose], [Notes], [Phone], [Street], [CompanyName], [CreatedOn], [StartDate], [EndDate], [DocumentType], [DocumentStatus], [UserID], [MultimediaAnchorID], [StateChangeAnchorID], [NotificationAnchorID], [WorkRequestID]) VALUES (3, N'VBV', N'BVCBVC', N'435435435', N'ULICAAA', N'dfdfs', CAST(N'2021-06-07T00:14:17.9900000' AS DateTime2), CAST(N'2021-05-31T00:00:00.0000000' AS DateTime2), CAST(N'2021-07-05T00:00:00.0000000' AS DateTime2), N'APPROVED', N'UNPLANNED', 5, NULL, NULL, NULL, 2)
SET IDENTITY_INSERT [dbo].[WorkPlans] OFF
GO
SET IDENTITY_INSERT [dbo].[SafetyDocuments] ON 

INSERT [dbo].[SafetyDocuments] ([ID], [Details], [Notes], [Phone], [OperationCompleted], [TagsRemoved], [GroundingRemoved], [Ready], [CreatedOn], [DocumentType], [DocumentStatus], [UserID], [MultimediaAnchorID], [StateChangeAnchorID], [NotificationAnchorID], [WorkPlanID]) VALUES (2, N'', N'', N'5435', 1, 0, 0, 1, CAST(N'2021-06-06T22:26:41.4880000' AS DateTime2), N'UNPLANNED', N'APPROVED', 17, 8, 4, 8, 2)
SET IDENTITY_INSERT [dbo].[SafetyDocuments] OFF
GO
SET IDENTITY_INSERT [dbo].[StateChangeHistories] ON 

INSERT [dbo].[StateChangeHistories] ([ID], [ChangeDate], [DocumentStatus], [StateChangeAnchorID], [UserID]) VALUES (1, CAST(N'2021-06-07T00:05:12.4400000' AS DateTime2), N'DRAFT', 1, 5)
INSERT [dbo].[StateChangeHistories] ([ID], [ChangeDate], [DocumentStatus], [StateChangeAnchorID], [UserID]) VALUES (2, CAST(N'2021-06-07T00:08:24.2100000' AS DateTime2), N'DRAFT', 2, 5)
INSERT [dbo].[StateChangeHistories] ([ID], [ChangeDate], [DocumentStatus], [StateChangeAnchorID], [UserID]) VALUES (3, CAST(N'2021-06-07T00:10:48.4100000' AS DateTime2), N'APPROVED', 2, 17)
INSERT [dbo].[StateChangeHistories] ([ID], [ChangeDate], [DocumentStatus], [StateChangeAnchorID], [UserID]) VALUES (4, CAST(N'2021-06-07T00:15:01.3066667' AS DateTime2), N'DRAFT', 3, 17)
INSERT [dbo].[StateChangeHistories] ([ID], [ChangeDate], [DocumentStatus], [StateChangeAnchorID], [UserID]) VALUES (5, CAST(N'2021-06-07T00:16:31.5466667' AS DateTime2), N'APPROVED', 1, 17)
INSERT [dbo].[StateChangeHistories] ([ID], [ChangeDate], [DocumentStatus], [StateChangeAnchorID], [UserID]) VALUES (6, CAST(N'2021-06-07T00:18:38.7700000' AS DateTime2), N'APPROVED', 3, 17)
INSERT [dbo].[StateChangeHistories] ([ID], [ChangeDate], [DocumentStatus], [StateChangeAnchorID], [UserID]) VALUES (7, CAST(N'2021-06-07T00:26:55.8000000' AS DateTime2), N'DRAFT', 4, 17)
INSERT [dbo].[StateChangeHistories] ([ID], [ChangeDate], [DocumentStatus], [StateChangeAnchorID], [UserID]) VALUES (8, CAST(N'2021-06-07T00:27:30.1700000' AS DateTime2), N'DENIED', 4, 17)
INSERT [dbo].[StateChangeHistories] ([ID], [ChangeDate], [DocumentStatus], [StateChangeAnchorID], [UserID]) VALUES (9, CAST(N'2021-06-07T00:27:32.0233333' AS DateTime2), N'APPROVED', 4, 17)
INSERT [dbo].[StateChangeHistories] ([ID], [ChangeDate], [DocumentStatus], [StateChangeAnchorID], [UserID]) VALUES (10, CAST(N'2021-06-07T11:15:37.0633333' AS DateTime2), N'DRAFT', 5, 5)
SET IDENTITY_INSERT [dbo].[StateChangeHistories] OFF
GO

SET IDENTITY_INSERT [dbo].[Resolutions] ON 

INSERT [dbo].[Resolutions] ([ID], [Cause], [Subcause], [Construction], [Material], [IncidentID]) VALUES (1, N'WEATHER', N'SNOW', N'SURFACE', N'PLASTICS', 1)
SET IDENTITY_INSERT [dbo].[Resolutions] OFF
GO
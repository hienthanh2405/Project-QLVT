CREATE VIEW [dbo].[V_DS_PHANMANH]
AS
SELECT TENCN=PUBS.description, TENSERVER=SUBS.subscriber_server
FROM dbo.sysmergepublications PUBS, dbo.sysmergesubscriptions SUBS
WHERE PUBS.pubid=SUBS.pubid AND PUBS.publisher <> SUBS.subscriber_server
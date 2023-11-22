declare @fromDate datetime = '2020-07-04 00:00:00:000'
declare @toDate datetime = '2023-12-30 00:00:00:000'

SELECT [Check-In Date], sum([Deposit Paid])
from [Reservations]
where [Check-In Date] between @fromDate and @toDate 
group by [Check-In Date]
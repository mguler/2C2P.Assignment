# 2C2P.Assignment
2C2P Assignment Project

#API url's and sample requests:

http://localhost:5000/Transaction/GetAllTransactionsByCurrency?currency=USD
http://localhost:5000/Transaction/GetAllTransactionsByDateRange?begin=2019-10-21&end=2020-10-21
http://localhost:5000/Transaction/GetAllTransactionsByStatus?status=D

connections strings are in ;
1) 2c2p.Assignment.Data.Context -> DataContextFactory.cs (design time)
2) 2c2p.Assignment.WebApp -> Startup.cs (runtime)

#Technologies;

1)Asp.net Core 5.0

2)Entityframework Core 5.0

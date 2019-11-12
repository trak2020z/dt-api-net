﻿using AI_NETCORE_API.Models.Objects;
using AI_NETCORE_API.Models.Response.ExecutingTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AI_NETCORE_API.Models.Response.Transactions
{
    public class GetTransactionsByUserIdResponseModel
    {
        public IList<TransactionModel> Transactions { get; set; }
        public ExecutionDetails ExecDetails { get; set; }
    }
}

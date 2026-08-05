using System;
using System.Collections.Generic;
using System.Text;

namespace LoanService.Domain.Aggregates
{
    public enum LoanStatus
    {
        Draft = 1,       // Черновик
        Submitted = 2,   // На рассмотрении
        Approved = 3,    // Одобрена (готова к сбору средств от инвесторов)
        Active = 4,      // Деньги выданы, займ выплачивается
        Repaid = 5,      // Закрыл кредит
        Rejected = 6     // Отклонено
    }
}

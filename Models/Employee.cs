using System;
using System.Collections.Generic;

namespace RPBDIS_lab2.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FullName { get; set; }

    public string? Position { get; set; }

    public DateOnly? HireDate { get; set; }

    public virtual ICollection<LoanedBook> LoanedBooks { get; set; } = new List<LoanedBook>();
}

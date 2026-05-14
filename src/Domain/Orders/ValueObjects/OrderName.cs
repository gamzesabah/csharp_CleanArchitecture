using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Orders.ValueObjects;

public class OrderName
{
    public string Value { get; private set; }

    public OrderName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new Exception("OrderName boş olamaz");
        }
        if (value.Length < 3)
        {
            throw new Exception("OrderName en az 3 karakter olmalı");
        }
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}

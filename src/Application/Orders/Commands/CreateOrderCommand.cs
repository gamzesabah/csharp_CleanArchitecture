using System;
using System.Collections.Generic;
using System.Text;
using Application.Abstractions.Messaging;
using Application.Orders.Dtos;

namespace Application.Orders.Commands;

public sealed record CreateOrderCommand(string Name, decimal TotalAmount): ICommand<OrderDto>;

using System;
using System.Collections.Generic;
using System.Text;
using Application.Abstractions.Messaging;
using Application.Orders.Dtos;
using SharedKernel;

namespace Application.Orders.Queries;

public class GetOrderListQuery : IQuery<List<OrderDto>>
{
}

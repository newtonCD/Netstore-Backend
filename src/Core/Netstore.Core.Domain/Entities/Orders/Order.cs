﻿using Netstore.Core.Domain.Entities.Base;
using System;

namespace Netstore.Core.Domain.Entities.Orders;

public class Order : Entity
{
    public long OrderId { get; set; }
    public Guid OrderCode { get; set; }
    public int StatusId { get; set; }
    public Status OrderStatus { get; set; }
}
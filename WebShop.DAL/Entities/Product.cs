﻿namespace WebShop.DAL.Entities;

public class Product : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public decimal Price { get; set; }
}

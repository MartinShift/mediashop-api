﻿namespace MediaShop.Data.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }

    public virtual ICollection<Product> Products { get; set; }
}

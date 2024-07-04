﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Domain.Entities.Relationships;

public class WishlistBook
{
    [Required, Column("wishlist_id")]
    public int WishlistId { get; set; }
    public Wishlist? Wishlist { get; set; }

    [Required, Column("book_id")]
    public int BookId { get; set; }
    public Book? Book { get; set; }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
	public class Item
	{
        public int ItemId { get; set; }

        //"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}" allows only characters, numbers, periods, hyphens, and spaces and enforces a length constraint of 2 to 20 characters.
        // ErrorMessage: provides a custom error message to be displayed if the validation fails. In this case, it tells the user that the
        // name must consist of numbers or letters and be between 2 to 20 characters.
        // Display(Name = "Item name")It sets the display name or label for the property to "Item name." This can be useful for
        // generating user-friendly labels in forms or views.
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The Name must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Item name")]
        public string Name { get; set; } = string.Empty;

        // Means that the minimum accepted value is 0.01, to the max value that double.MaxValue holds and throws the errormessage if that fails
        [Range(0.01, double.MaxValue, ErrorMessage = "The Price must be greater than 0.")]
        public decimal Price { get; set; }

        // Means that the max value is 200 characters long in the description
        [StringLength(200)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
        // navigation property
        public virtual List<OrderItem>? OrderItems { get; set; }
    }
}


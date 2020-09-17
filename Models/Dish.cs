using System;
using System.ComponentModel.DataAnnotations;

namespace CSCRUDelicious.Models
{
    public class Dish
    {
        [Key]
        public int DishId { get; set; }         //dont forget PK from MySQL DB!! and mark it [Key] for the framework


        [Required(ErrorMessage = "...missing!")]
        [Display(Name = "Name of Dish")]
        public string Name { get; set; }


        [Required(ErrorMessage = "...missing!")]
        [Display(Name = "Chef's Name")]
        public string Chef { get; set; }


        public int Tastiness { get; set; }


        [Required(ErrorMessage = "...missing!"), Range(1, 9000, ErrorMessage = "... no way!?!")]
        [Display(Name = "# of Calories")]
        public int Calories { get; set; }


        [Required(ErrorMessage = "...What's this dish about?"), MinLength(10, ErrorMessage = "...Tell me more? (10 char min)")]
        public string Description { get; set; }


        //[DataType(DataType.Date)]     
        //Dont need, not displaying
        public DateTime CreatedAt { get; set; } //-  dont want it to be updated EVERY TIME!!

        //[DataType(DataType.Date)]     
        //Dont need, not displaying
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Dish()
        {
            this.CreatedAt = DateTime.Now;
        }

    }
}
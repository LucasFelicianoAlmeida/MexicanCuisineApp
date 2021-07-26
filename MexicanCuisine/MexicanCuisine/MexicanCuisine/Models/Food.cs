
    using System;
    using System.Collections.Generic;
namespace MexicanCuisine
{
    
    public partial class Food
    {
        public int Id { get; set; }
        public int IdTypeMeal { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string ImgOutline { get; set; }
        public string ImgBlack { get; set; }
    
        public virtual TypeMeal TypeMeal { get; set; }
    }
}


using System;

namespace TestCars.Model
{
    public class Car
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Equipment { get; set; }
        public float Price { get; set; }

        public Car(string brand, string model, string color, string equipment, float price)            
        {
            Brand = brand;
            Model = model;
            Color = color;
            Equipment = equipment;
            Price = price;
        }
    }
}

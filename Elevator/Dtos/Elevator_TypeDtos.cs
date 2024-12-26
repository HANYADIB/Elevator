namespace Elevator.Dtos
{
    public class Elevator_TypeDtos
    {
        public List<Elevator_Objes> Elevator_Objes { get; set; }
    }
    public class Elevator_Objes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Types__Objes> Types__Objes { get; set; }

    }
    public class Types__Objes
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

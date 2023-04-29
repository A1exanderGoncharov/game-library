namespace Web.Models
{
    public class DeleteConfirmationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public DeleteConfirmationViewModel(int id, string name, string controllerName, string actionName)
        {
            Id = id;
            Name = name;
            ControllerName = controllerName;
            ActionName = actionName;
        }
    }
}

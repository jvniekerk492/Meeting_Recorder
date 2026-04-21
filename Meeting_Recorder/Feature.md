Create a view factory that decouples the view creation from the main application logic. 
This factory will be responsible for creating and managing the views used in the application, allowing for better separation of concerns and easier maintenance.
Also decouple the view from the viewmodel so views can be reused with different viewmodels if needed. 
## Use a singleton pattern for the view factory to ensure that there is only one instance of the factory throughout the application,
## providing a centralized point for view management.
## Use a simple factory method pattern to create views based on an enum or for better maintainability.
## Factory return type of view, which is of type IView, the view should always bind to properties on the ViewModel.
## For decoupling views from viewmodels, should views accept their viewmodel through a constructor parameter
## View folder should contain the name of the main feature of the view and the view model, for example: Recorder.xaml and RecorderViewModel.cs.
1. Define a Enum that represents the different views in the application, such as Recorder, Settings, etc.
2. Define a ViewFactory class that will be responsible for creating and managing views.
2.1. Implement a method in the view factory class that that takes a Enum and returns the corresponding view.
2.2 Use a dictionary to map Enum to their corresponding view classes, allowing for easy retrieval and instantiation of views.
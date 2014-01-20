WPF contains great things like Bindings and interface INotifyPropertyChanged. These two things allow to forget the hell from WinForms and almost all previous UI technologies, where you spend 90% of your time for developing and maintenaning UI code (like updates in UI fields, hide or show some controls etc.) If you still do not know what it is (hmmm, you develop in WPF and don’t know what is “binding”?), you can read about them, for example, [here](http://www.codeproject.com/KB/WPF/BeginWPF5.aspx). And I want to talk about a problem named “magic-strings”.

## How it was

So, how usually properties’ implementations look when you use INotifyPropertyChanged? Something like this:

<pre class="brush: c#">

	public class Author
		: INotifyPropertyChanged
	{
		#region Properties
		
		#region FirstName

		private string _FirstName;

		public string FirstName
		{
			get { return _FirstName; }
			set
			{
				if (_FirstName => value)
					return;

				_FirstName = value;
				RaisePropertyChanged("FirstName");
			}
		}

		#endregion

		#endregion

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}

</pre>

The problem is in the line 'RaisePropertyChanged("FirstName")' – there is “magic-string” – “FirstName”. You can make a typo in this string or when you will change property’s name using refactoring – this string will not change. Also, there is yet one place, when this “magic-string” is used – PropertyChanged event’s handler. It looks like this:

<pre class="brush: c#">

	public class AuthorViewModel
	{
		public AuthorViewModel()
		{
			Author = new Author();
			Author.PropertyChanged += Author_PropertyChanged;
		}

		public Author Author { get; private set; }

		void Author_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "FirstName":
					MessageBox.Show("Author.FirstName was changed");
					break;
			}
		}
	}

</pre>

The problem is the same – you can make a typo or you can change name of this property later. Also, I don’t like how “switch” code looks like. So, what to do?

The answer is – Lambdas!

## How it is

I want to warn all optimizators and other perfectionists. Yes, I know that my code below works slower than “magic-strings”. But I prefer more robust code than faster one. If your code is fast but it works wrong – do you really need it?

I saw a lot of articles where another guys describe the same decision with RaisePropertyChanged() method, and Prism also uses this approach. But I never saw before usage of the same principle in PropertyChanged event’s handler.

OK, sorry for a lot of words, here is how the same code looks after.

<pre class="brush: c#">

	public class Author
		: NotificationObject
	{
		#region Properties

		#region FirstName

		private string _FirstName;

		public string FirstName
		{
			get { return _FirstName; }
			set
			{
				if (_FirstName == value)
					return;

				_FirstName = value;
				RaisePropertyChanged(() => FirstName);
			}
		}

		#endregion

		#endregion
	}

</pre>

Here is actually not too much changes. We move implementation of INotifyPropertyChanged to base class NotificationObject and (attention!) remove “magic-string”. So, now we can refactor Author class and do anything you want with FirstName property’s name! And PropertyChanged event’s handler changes to:

<pre class="brush: c#">

	void Author_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		On.PropertyChanged(e, (Author a) => a.FirstName, () => MessageBox.Show("Author.FirstName was changed"));
	}

</pre>

It’s also now simple and robust.

All necessary code you can download by links in the end of this article.

## Fast property implementation

It’s terrible to write every time the same code for implementation of properties. So, if you use ReSharper, you can download code template for it, and implement properties with just some keystrokes. All you should do:

1. Type “pcp” (without quotes) + press Tab
2. Type property’s type + press Tab
3. Type property’s name + press Tab
4. It’s everything you should to do!

You can import that code template by VS main menu –> ReSharper –> Live Templates… –> Import…

## Typical use cases

### Raise property change notification after changes of another property

For example, class Author has properties FirstName, LastName and FullName, which is equal to FirstName + LastName. When FirstName or LastName is changed, of course you want to notify about FullName changes also. You can do this 2 ways:

* Add additional code to FirstName’s and LastName’s setters:

<pre class="brush: c#">

		public string FirstName
		{
			get { return _FirstName; }
			set
			{
				if (_FirstName == value) return;
				_FirstName = value;
				RaisePropertyChanges(() => FirstName);
				RaisePropertyChanges(() => FullName);
			}
		}

</pre>

* Add additional code to Author.PropertyChanges event’s handler:

<pre class="brush: c#">

		public class Author
		{
			public Author
			{
				PropertyChanged += Author_PropertyChanges;
			}
			void Author_PropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				On.PropertyChanged(e, () => FirstName, () => RaisePropertyChanged(() => FullName));
				On.PropertyChanged(e, () => LastName, () => RaisePropertyChanged(() => FullName));
			}
		}

</pre>

Maybe there are some more code, but I think it’s more understandable. All additional notifications and actions on properties’ changes are now in separate place.

### Additional action on property’s changes

For example, you want to get from database some additional information after setting ID.

<pre class="brush: c#">

	public class Author
	{
		public Author
		{
			PropertyChanged += Author_PropertyChanges;
		}

		void Author_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			On.PropertyChanged(e, () => ID, UpdateData);
		}

		void UpdateData()
		{
			// Your code here
		}
	}

</pre>

### Additional actions in parent class

<pre class="brush: c#">

	public class AuthorViewModel
	{
		public Author Author { get; private set; }

		public AuthorViewModel
		{
			Author.PropertyChanged += Author_PropertyChanged;
		}

		void Author_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			On.PropertyChanged&lt;Author>(e, a => a.ID, UpdateData);
			// Or you can use this syntax:
			// On.PropertyChanged(e, (Author a) => a.ID, UpdateData);
		}

		// This method invokes when Author.ID changes
		void UpdateData()
		{
			// Your code here
		}
	}

</pre>

## Downloads

* [On.cs](/Content/20111029/On.cs)
* [NotificationObject.cs](/Content/20111029/NotificationObject.cs)
* [ReSharper code template](/Content/20111029/ReSharper_PropertyChangedProperty.xml)
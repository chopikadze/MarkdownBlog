## Intro

.NET Framework has a very useful class – ObservableCollection, which is almost usual collection, but has great feature – it supports notification about collection's changes. This class is used very wide in WPF with it’s binding engine. But.. I needed some additional features, which aren’t supported by it. So – this is result of work on “Extended ObservableCollection” – ObservableCollectionEx.

I will use additional class TestItem for showing examples of usage of ObervableCollectionEx. The code of this class you can download by the link in the end of the article (as the code of ObservableCollectionEx itself). All you need to know actually that it has property Value of type int and it supports INotifyPropertyChanged interface, so user of this class can know about Value’s changes.

## Features of ObservableCollectionEx

### ObservableCollection

First of all, this is usual ObservableCollection. It supports all it’s features – as usual List, as Collection, Enumerable, notifications about collection changes etc. A lot of code was written after exploring internals of Microsoft’s ObservableCollection with Reflector. So, if you use anywhere ObservableCollection – you can easily switch to using ObservableCollectionEx – just by changing type of variables and constructors’ calls.

### Notification about Item’s properties’ changes

ObservableCollection notifies about changes of items set, it notifies about properties’ changes of itself also (for example, about changes of Count property or Item). But it doesn’t notify about changes of properties of its items. And actually this is very useful thing in WPF development. So, I had added event ItemPropertyChanged to my ObservableCollectionEx. You can attach event handler to it and check when item of collection is changed. Something like:

<pre class="brush: c#">

	var collection = new ObservableCollectionEx();
	collection.ItemPropertyChanged += Collection_ItemPropertyChanged;

	void Collection_ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		var msg = string.Format("Property '{0}' of '{1}' was changed",
		                         e.PropertyName, sender);
		MessageBox.Show(msg);
	}

</pre>

### Thread-safe collection

Almost all collections in .NET Framework are not thread-safe. I’m sure that this was done for performance reasons, but.. Sometimes you need thread-safety. Especially in WPF, when one thread (UI) can display items from collection, second thread (receiver from some external application) can add items to collection, and third thread (some background worker) can remove items from collection. So, ObservableCollectionEx is thread-safe now! You shouldn’t do anything – it’s “out ofm the box”. Just use it and you can be sure, that if one thread is using some method (for example, Add()), no one can use another method (for example, Remove()).

### Filter

Another feature which I’d like to see in ObservableCollection (I want too much, don’t I?) is filtering. Very often I have 2 scenarios – I want to show only part of collection in the screen (for example, only failed executions of something) or I want to separate one collection to several ItemControls – for example, show failed executions in one and successful in another. And.. I want to be sure that items are displayed in right ItemsControl, even if they are changed dynamically – if there are new items, or if existed items are changed, so they should be removed from collection or they should jump to another ItemsControl. ObservableCollectionEx supports filtering. Even more – this filter is thread-safe, it monitors changes of items, and it can even use another ObservableCollection as source of items. Here is the example of usage of Filter:

<pre class="brush: c#">

	var collection = new ObservableCollection();
	collection.Filter = p => p.Value > 10;

	var item1 = new TestItem(0);
	var item2 = new TestItem(10);
	var item3 = new TestItem(20);

	collection.Add(item1);
	collection.Add(item2);
	collection.Add(item3);

</pre>

After execution of the code above the collection will contain only 1 item actually – with value 20. But – collection will monitor changes in all 3 items. So, for example, if we execute:

<pre class="brush: c#">

	item2.Value = 30;

</pre>

collection will have 2 items – we didn’t add any item to it, but one item, which is “virtually” there, now passes filter. So, now collection has 2 items – item3 (20) and item2 (30). The same, if we execute:

<pre class="brush: c#">

	item3.Value = 5;

</pre>

this item will be removed from collection. But monitoring is still working. So, if we change value of item3 back to 30, it will come back to collection.

Yet one way of using Filter is to use another observable collection as source of this collection, like:

<pre class="brush: c#">

	var sourceCollection = new ObservableCollection&lt;TestItem>();
	var filteredCollection = new ObservableCollectionEx&lt;TestItem>();
	filteredCollection.Source = sourceCollection;
	filteredCollection.Filter = p => p.Value > 10;

</pre>

From now, filteredCollection will contain only items from sourceCollection which passes filter criterion. And even if sourceCollection will change or items’ Values will change – filteredCollection will still contain all (and only) needed items. As source any IEnumerable<T> collection can be used. Sure, in this case ObservableCollectionEx cannot monitor changes of source collection, but if it’s static – it’s not a problem. For example, ObservableCollectionEx can be used as a filter layer in FilteredTextBox.

## Source files

* [ObservableCollectionEx.cs](/Content/20120314/ObservableCollectionEx.cs)
* [TestItem.cs](/Content/20120314/TestItem.cs)

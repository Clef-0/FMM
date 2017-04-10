using System;
using System.Collections.ObjectModel;

namespace ListViewDragDropManagerDemo
{
	enum TaskDuration
	{
		Unknown,
		VeryShort,
		Short,
		Medium,
		Long,
		VeryLong
	}

	class Task
	{
		public static ObservableCollection<Task> CreateTasks()
		{
			ObservableCollection<Task> list = new ObservableCollection<Task>();

			list.Add( new Task( TaskDuration.VeryShort, "Take out trash", "The trash can is full again.", false ) );
			list.Add( new Task( TaskDuration.Short, "Clean the bathroom", "It's my turn to scrub the tub. :(", true ) );
			list.Add( new Task( TaskDuration.Medium, "Write CodeProject article", "Need to write a CP article about ListViewDragDropManager.", true ) );
			//list.Add( null ); // Test what happens when there is a null item in the ListView.
			list.Add( new Task( TaskDuration.VeryLong, "Learn 3D programming", "Learn how to create slick UIs using the WPF 3D APIs.", false ) );
			list.Add( new Task( TaskDuration.Unknown, "Get in touch with Tom", "It's been a while since I've spoken with him.", false ) );

			return list;
		}

		TaskDuration duration;
		string name;
		string description;
		bool finished;

		public Task( TaskDuration duration, string name, string description, bool finished )
		{
			this.duration = duration;
			this.name = name;
			this.description = description;
			this.finished = finished;
		}

		public bool Finished
		{
			get { return this.finished; }
			set { this.finished = value; }
		}

		public TaskDuration Duration
		{
			get { return this.duration; }
		}

		public string Name
		{
			get { return this.name; }
		}

		public string Description
		{
			get { return this.description; }
		}
	}
}

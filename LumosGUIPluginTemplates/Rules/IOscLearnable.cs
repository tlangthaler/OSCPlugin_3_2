using System;
using Ventuz.OSC;

namespace OSCGUIPlugin
{
	public interface IOscLearnable
	{
		event EventHandler LearningFinished;
		string LearnStatus
		{
			get;
		}
		void BeginLearn();
		void CancelLearn();
		bool TryLearnMessage(OscElement m);
	}
}

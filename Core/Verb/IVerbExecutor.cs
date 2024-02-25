
using System;
using System.Collections.Generic;

namespace CrashEdit {

    public interface IVerbExecutor {

        void ExecuteVerb(Verb verb);

        void ExecuteVerbChoice(List<Verb> verbs);

    }

}

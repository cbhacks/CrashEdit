
using System;

namespace CrashEdit {

    public sealed class Walker {

        public Controller? Cursor { get; set; }

        public bool MoveToParent() {
            if (Cursor == null)
                throw new InvalidOperationException();

            if (Cursor.Parent == null) {
                return false;
            } else {
                Cursor = Cursor.Parent;
                return true;
            }
        }

        public bool MoveToFirstChild() {
            if (Cursor == null)
                throw new InvalidOperationException();

            foreach (var ctlrGroup in Cursor.SubcontrollerGroups) {
                if (ctlrGroup.Members.Count > 0) {
                    Cursor = ctlrGroup.Members[0];
                    return true;
                }
            }
            return false;
        }

        public bool MoveToLastChild() {
            if (Cursor == null)
                throw new InvalidOperationException();

            for (int i = Cursor.SubcontrollerGroups.Count - 1; i >= 0; i--) {
                var ctlrGroup = Cursor.SubcontrollerGroups[i];
                if (ctlrGroup.Members.Count > 0) {
                    Cursor = ctlrGroup.Members[ctlrGroup.Members.Count - 1];
                    return true;
                }
            }
            return false;
        }

        public bool MoveToNextSibling() {
            if (Cursor == null)
                throw new InvalidOperationException();

            if (Cursor.ParentGroup == null) {
                // Orphan, no siblings.
                return false;
            }

            int ctlrIdx = Cursor.ParentGroup.Members.IndexOf(Cursor);
            if (ctlrIdx == -1) {
                // Controller under cursor disappeared from its group.
                throw new InvalidOperationException();
            }

            if (ctlrIdx + 1 < Cursor.ParentGroup.Members.Count) {
                // Found a sibling in the same group.
                Cursor = Cursor.ParentGroup.Members[ctlrIdx + 1];
                return true;
            }

            var parent = Cursor.ParentGroup.Owner;
            int grpIdx = parent.SubcontrollerGroups.IndexOf(Cursor.ParentGroup);
            if (grpIdx == -1) {
                // Controller group disappeared from its owner.
                throw new InvalidOperationException();
            }

            for (grpIdx++; grpIdx < parent.SubcontrollerGroups.Count; grpIdx++) {
                var ctlrGroup = parent.SubcontrollerGroups[grpIdx];
                if (ctlrGroup.Members.Count > 0) {
                    // Found a non-empty controller group after us.
                    Cursor = ctlrGroup.Members[0];
                    return true;
                }
            }

            // No later siblings.
            return false;
        }

        public bool MoveToPreviousSibling() {
            if (Cursor == null)
                throw new InvalidOperationException();

            if (Cursor.ParentGroup == null) {
                // Orphan, no siblings.
                return false;
            }

            int ctlrIdx = Cursor.ParentGroup.Members.IndexOf(Cursor);
            if (ctlrIdx == -1) {
                // Controller under cursor disappeared from its group.
                throw new InvalidOperationException();
            }

            if (ctlrIdx - 1 >= 0) {
                // Found a sibling in the same group.
                Cursor = Cursor.ParentGroup.Members[ctlrIdx - 1];
                return true;
            }

            var parent = Cursor.ParentGroup.Owner;
            int grpIdx = parent.SubcontrollerGroups.IndexOf(Cursor.ParentGroup);
            if (grpIdx == -1) {
                // Controller group disappeared from its owner.
                throw new InvalidOperationException();
            }

            for (grpIdx--; grpIdx >= 0; grpIdx--) {
                var ctlrGroup = parent.SubcontrollerGroups[grpIdx];
                if (ctlrGroup.Members.Count > 0) {
                    // Found a non-empty controller group before us.
                    Cursor = ctlrGroup.Members[ctlrGroup.Members.Count - 1];
                    return true;
                }
            }

            // No earlier siblings.
            return false;
        }

        public bool MoveToNextDFS() {
            var savedCursor = Cursor;

            if (MoveToFirstChild()) {
                return true;
            }

            while (!MoveToNextSibling()) {
                if (!MoveToParent()) {
                    //Cursor = savedCursor;
                    return false;
                }
            }
            return true;
        }

        public bool MoveToPreviousDFS() {
            var savedCursor = Cursor;

            if (MoveToPreviousSibling()) {
                while (MoveToLastChild()) {}
                return true;
            }

            if (MoveToParent()) {
                return true;
            } else {
                //Cursor = savedCursor;
                return false;
            }
        }

    }

}

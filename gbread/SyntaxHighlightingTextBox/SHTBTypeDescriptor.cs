using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GBRead
{
    ///
    /// These classes are required for correct data binding to the Text property of SyntaxHighlightingTextbox.
    /// 
    class SHTBDescriptionProvider : TypeDescriptionProvider
    {
        private static TypeDescriptionProvider defaultTypeProvider = TypeDescriptor.GetProvider(typeof(SyntaxHighlightingTextBox));

        public SHTBDescriptionProvider()
            : base(defaultTypeProvider)
        {  
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            ICustomTypeDescriptor defaultDescriptor = base.GetTypeDescriptor(typeof(SyntaxHighlightingTextBox), instance);
            return new SHTBTypeDescriptor(defaultDescriptor, instance);
        }
    }

    class SHTBTypeDescriptor : CustomTypeDescriptor
    {
        ICustomTypeDescriptor parent;
        object instance;

        public SHTBTypeDescriptor(ICustomTypeDescriptor parent, object instance)
            : base(parent)
        {
            this.parent = parent;
            this.instance = instance;
        }

        public override string GetComponentName()
        {
            var ctrl = (instance as Control);
            return ctrl == null ? null : ctrl.Name;
        }

        public override EventDescriptorCollection GetEvents()
        {
            var coll = base.GetEvents();
            var list = new EventDescriptor[coll.Count];

            for (int i = 0; i < coll.Count; i++)
                if (coll[i].Name == "TextChanged")//instead of TextChanged slip BindingTextChanged for binding
                    list[i] = new FooTextChangedDescriptor(coll[i]);
                else
                    list[i] = coll[i];

            return new EventDescriptorCollection(list);
        }
    }

    class FooTextChangedDescriptor : EventDescriptor
    {
        public FooTextChangedDescriptor(MemberDescriptor desc)
            : base(desc)
        {
        }

        public override void AddEventHandler(object component, Delegate value)
        {
            (component as SyntaxHighlightingTextBox).BindingTextChanged += value as EventHandler;
        }

        public override Type ComponentType
        {
            get { return typeof(SyntaxHighlightingTextBox); }
        }

        public override Type EventType
        {
            get { return typeof(EventHandler); }
        }

        public override bool IsMulticast
        {
            get { return true; }
        }

        public override void RemoveEventHandler(object component, Delegate value)
        {
            (component as SyntaxHighlightingTextBox).BindingTextChanged -= value as EventHandler;
        }
    }
}

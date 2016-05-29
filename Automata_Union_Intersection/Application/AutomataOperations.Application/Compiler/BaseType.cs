using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataOperations.Application.Compiler
{
    public abstract class BaseType
    {
        private BaseType _posterior;
        private ECustomType _typeProcess;
        private ECustomType _typeResult;

        public BaseType()
        {
        }

        protected BaseType(BaseType posterior)
        {
            _posterior = posterior;
        }

        protected BaseType(BaseType posterior, ECustomType typeProcess, ECustomType typeResult)
            : this(posterior)
        {
            _typeProcess = typeProcess;
            _typeResult = typeResult;
        }

        public CustomType RecognizeType(TypeToRecognize typeToRecognize)
        {
            CustomType result;

            if (typeToRecognize.Type == _typeProcess)
            {
                result = CreateType(typeToRecognize, _typeResult);

                typeToRecognize.Value.Clear();
            }
            else
            {
                result = Proceed(typeToRecognize);
            }

            return result;
        }

        private CustomType Proceed(TypeToRecognize typeToRecognize)
        {
            CustomType result = null;

            if (_posterior != null)
            {
                result = _posterior.RecognizeType(typeToRecognize);
            }

            return result;
        }

        private CustomType CreateType(TypeToRecognize typeToRecognize, ECustomType type)
        {
            CustomType result = new CustomType(type, typeToRecognize.Value.ToString());

            return result;
        }
    }
}

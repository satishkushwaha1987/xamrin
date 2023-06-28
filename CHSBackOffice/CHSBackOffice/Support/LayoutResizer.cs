using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Support
{
    public class LayoutResizer
    {
        #region "Private Fields"
        private static double _demensionContainer;
        private static double _demensionItem;
        private static int _count;
        private double _minSpace = 5;
        #endregion

        #region "PUBLIC PROPS"
        public static LayoutResizer Instance = new LayoutResizer();
        #endregion

        #region "Public Method"
        public double InitColumnSpace(double demensionContainer, double demensionItem)
        {
            _demensionContainer = demensionContainer;
            _demensionItem = demensionItem;
            var column = Math.Truncate(_demensionContainer / (_demensionItem + _minSpace));
            var _spaceContainer = _demensionContainer - (_demensionItem * column);

            return _spaceContainer / (column + 1);
        }

        public double InitColumnSpace(double demensionContainer, double demensionItem, double minSpaceItem)
        {
            _demensionContainer = demensionContainer;
            _demensionItem = demensionItem;
            _minSpace = minSpaceItem;
            var optimalColumnCount = Math.Truncate(_demensionContainer / (_demensionItem + _minSpace));
            var _spaceContainer = _demensionContainer - (_demensionItem * optimalColumnCount);

            return _spaceContainer / (optimalColumnCount + 1);
        }

        public double InitRowSpace(double demensionContainer, double demensionItem, int count)
        {
            _demensionContainer = demensionContainer;
            _demensionItem = demensionItem;
            var _spaceContainer = _demensionContainer - (_demensionItem * count);
            return _spaceContainer / (count + 1);
        }

        public double InitDemension(double demensionContainer, int count, double minSpaceItem)
        {
            _demensionContainer = demensionContainer;
            _count = count;
            _minSpace = minSpaceItem;
            var widthItem = Math.Truncate((_demensionContainer - _minSpace * count) / count);
            return widthItem;
        }
        #endregion
    }
}

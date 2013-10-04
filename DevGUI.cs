/// <summary>
/// DevGUI 
/// Sub-Class of DevManager
/// </summary>
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DevelopManager
{
	public static class DevGUI
	{
		public static Rect BeginBorder(Rect _rect)
		{
			return BeginBorder(_rect, new RectOffset(1,1,1,1), UnityEngine.Color.black, UnityEngine.Color.clear);
		}
		public static Rect BeginBorder(Rect _rect, RectOffset _borderSize)
		{
			return BeginBorder(_rect, _borderSize, UnityEngine.Color.black, UnityEngine.Color.clear);
		}
		public static Rect BeginBorder(Rect _rect, RectOffset _borderSize, UnityEngine.Color _border)
		{
			return BeginBorder(_rect, _borderSize, _border, UnityEngine.Color.clear);
		}
		public static Rect BeginBorder(Rect _rect, RectOffset _borderSize, UnityEngine.Color _border, UnityEngine.Color _background)
		{
			GUI.BeginGroup(_rect);
			GUIStyle _borderStyle = new GUIStyle();
			if( _border.Equals(UnityEngine.Color.clear) )
			{
				_border = UnityEngine.Color.black;
			}
			_borderStyle.normal.background = fillColor(_border);
			RectOffset _margin = new RectOffset(0,0,0,0);
			RectOffset _size = _borderSize;
			
			// top
			if( _size.top > 0 )
			GUI.Box(new Rect(	(_rect.x + _margin.left),
								(_rect.y + _margin.top),
								(_rect.width - _margin.left - _margin.right),
								(_size.top)
							),"",_borderStyle);
			// bottom
			if( _size.bottom > 0 )
			GUI.Box(new Rect(	(_rect.x + _margin.left),
								(_rect.height - _margin.bottom - _size.bottom),
								(_rect.width - _margin.left - _margin.right),
								(_size.bottom)
							),"",_borderStyle);
			// left
			if( _size.left > 0 )
			GUI.Box(new Rect(	(_rect.x + _margin.left),
								(_rect.y + _margin.top + _size.top ),
								(_size.left),
								(_rect.height - _size.top - _size.bottom - _margin.top - _margin.bottom)
							),"",_borderStyle);
			// right
			if( _size.right > 0 )
			GUI.Box(new Rect(	(_rect.width - _size.right - _margin.right),
								(_rect.y + _margin.top + _size.top ),
								(_size.right),
								(_rect.height - _size.top - _size.bottom - _margin.top - _margin.bottom)
							),"",_borderStyle);
			Rect _innerSize = new Rect(
					_margin.left + _size.left,
					_margin.top + _size.top,
					_rect.width-(_margin.left + _margin.right + _size.left + _size.right),
					_rect.height-(_margin.top + _margin.bottom + _size.top + _size.bottom));
			// background color
			if( _background.Equals(UnityEngine.Color.clear)==false )
			{
				GUIStyle _bgStyle = new GUIStyle();
				_bgStyle.normal.background = fillColor(_background);
				GUI.Box(_innerSize,"",_bgStyle);
			}
			// let's return a rect size of the inner Content, NOT include padding.
			// remember to call EndBorder(); to end this group.
			return _innerSize;
		}
		public static void EndBorder()
		{
			GUI.EndGroup();	
		}
		/// <summary>
		/// Fills the color in Texture2D
		/// </summary>
		/// <returns>
		/// Texture2D
		/// </returns>
		/// <param name='_bgColor'>
		/// color you want the texture to be.
		/// </param>
		public static Texture2D fillColor(UnityEngine.Color _bgColor)
		{
			Texture2D _bg = new Texture2D(1,1);
			_bg.SetPixel(1,1, _bgColor);
			_bg.Apply();
			return _bg;
		}
		/// <summary>
		/// Clones the GUI style.
		/// </summary>
		/// <returns>
		/// The GUI style.
		/// </returns>
		/// <param name='source'>
		/// Source.
		/// </param>
		public static GUIStyle CloneGUIStyle(GUIStyle source)
		{
			GUIStyle clone = new GUIStyle();
			clone.normal = source.normal;
			clone.hover = source.hover;
			clone.active = source.active;
			clone.onNormal = source.onNormal;
			clone.onHover = source.onHover;
			clone.onActive = source.onActive;
			clone.focused = source.focused;
			clone.onFocused = source.onFocused;
			clone.border = source.border;
			clone.margin = source.margin;
			clone.padding = source.padding;
			clone.overflow = source.overflow;
			clone.font = source.font;
			clone.imagePosition = source.imagePosition;
			clone.alignment = source.alignment;
			clone.wordWrap = source.wordWrap;
			clone.clipping = source.clipping;
			clone.contentOffset = source.contentOffset;
			clone.fixedWidth = source.fixedWidth;
			clone.fontSize = source.fontSize;
			clone.fontStyle = source.fontStyle;
			clone.fixedHeight = source.fixedHeight;
			clone.stretchWidth = source.stretchWidth;
			clone.stretchHeight = source.stretchHeight;
			// clone.lineHeight = source.lineHeight;
			return clone;
		}
	
	}
}//namespace

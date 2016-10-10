﻿using ReClassNET.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET
{
	class HotSpotTextBox : TextBox
	{
		private HotSpot hotSpot;
		public HotSpot HotSpot
		{
			get { return hotSpot; }
			set
			{
				if (hotSpot != value)
				{
					hotSpot = value;

					Left = hotSpot.Rect.Left + 2;
					Top = hotSpot.Rect.Top;
					Width = hotSpot.Rect.Width;
					Height = hotSpot.Rect.Height;

					MinimumWidth = Width;

					Text = hotSpot.Text.Trim();
				}
			}
		}

		public int MinimumWidth { get; set; }

		private FontEx font;
		public new FontEx Font
		{
			get { return font; }
			set
			{
				if (font != value)
				{
					font = value;

					base.Font = font.Font;
				}
			}
		}

		public HotSpotTextBox()
		{
			BorderStyle = BorderStyle.None;
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			if (Visible)
			{
				if (HotSpot != null)
				{
					Focus();
					Select(0, TextLength);
				}
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				hotSpot.Text = Text.Trim();

				var oldSize = hotSpot.Node.MemorySize;
				hotSpot.Node.Update(hotSpot);
				var newSize = hotSpot.Node.MemorySize;

				if (oldSize != newSize)
				{
					var classNode = hotSpot.Node as ClassNode;
					//classNode.ResizeTo(newSize);
				}

				Parent.Invalidate();

				Visible = false;

				return;
			}

			base.OnKeyDown(e);
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			var w = (TextLength + 1) * font.Width;
			if (w > MinimumWidth)
			{
				Width = w;
			}
		}
	}
}
﻿using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImGuiNET;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using OpenTK.Graphics.ES20;
using OpenTK.Windowing.Desktop;

namespace Chess.src
{
    public static class GuiImpl
    {
        private static bool _windowHasFocus = false;
        private static readonly bool[] MousePressed = new bool[] { false, false, false };
        private static readonly bool[] TouchDown = new bool[] { false, false, false };
        private static Texture _fontTexture = null;
        private static readonly Cursor[] MouseCursors = new Cursor[(int)ImGuiMouseCursor.COUNT];

        public static void Init(RenderWindow window, bool loadDefaultFont = true)
        {
            Init(window, window, loadDefaultFont);
        }

        public static void Init(Window window, RenderTarget target, bool loadDefaultFont = true)
        {
            Init(window, new Vector2f(target.Size.X, target.Size.Y), loadDefaultFont);
        }

        public static void Init(Window window, Vector2f displaySize, bool loadDefaultFont = true)
        {
            var context = ImGui.CreateContext();
            var io = ImGui.GetIO();

            io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;
            io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard; // Enable Keyboard Controls
            //io.ConfigFlags |= ImGuiConfigFlags_NavEnableGamepad;      // Enable Gamepad Controls
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable; // Enable Docking
            // io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable; // Enable Multi-Viewport / Platform Windows
            //io.ConfigFlags |= ImGuiConfigFlags_ViewportsNoTaskBarIcons;
            //io.ConfigFlags |= ImGuiConfigFlags_ViewportsNoMerge;

            // init keyboard mapping
            io.KeyMap[(int)ImGuiKey.Tab] = (int)Keyboard.Key.Tab;
            io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Keyboard.Key.Left;
            io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Keyboard.Key.Right;
            io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Keyboard.Key.Up;
            io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Keyboard.Key.Down;
            io.KeyMap[(int)ImGuiKey.PageUp] = (int)Keyboard.Key.PageUp;
            io.KeyMap[(int)ImGuiKey.PageDown] = (int)Keyboard.Key.PageDown;
            io.KeyMap[(int)ImGuiKey.Home] = (int)Keyboard.Key.Home;
            io.KeyMap[(int)ImGuiKey.End] = (int)Keyboard.Key.End;
            io.KeyMap[(int)ImGuiKey.Insert] = (int)Keyboard.Key.Insert;
            io.KeyMap[(int)ImGuiKey.Delete] = (int)Keyboard.Key.Delete;
            io.KeyMap[(int)ImGuiKey.Backspace] = (int)Keyboard.Key.Backspace;
            io.KeyMap[(int)ImGuiKey.Space] = (int)Keyboard.Key.Space;
            io.KeyMap[(int)ImGuiKey.Enter] = (int)Keyboard.Key.Enter;
            io.KeyMap[(int)ImGuiKey.Escape] = (int)Keyboard.Key.Escape;
            io.KeyMap[(int)ImGuiKey.A] = (int)Keyboard.Key.A;
            io.KeyMap[(int)ImGuiKey.C] = (int)Keyboard.Key.C;
            io.KeyMap[(int)ImGuiKey.V] = (int)Keyboard.Key.V;
            io.KeyMap[(int)ImGuiKey.X] = (int)Keyboard.Key.X;
            io.KeyMap[(int)ImGuiKey.Y] = (int)Keyboard.Key.Y;
            io.KeyMap[(int)ImGuiKey.Z] = (int)Keyboard.Key.Z;

            // init rendering
            io.DisplaySize = new Vector2(displaySize.X, displaySize.Y);

            // TODO: Implement
            // clipboard
            //io.SetClipboardTextFn = ClipboardText;
            //io.GetClipboardTextFn = getClipboadText;

            LoadMouseCursor(ImGuiMouseCursor.Arrow, Cursor.CursorType.Arrow);
            LoadMouseCursor(ImGuiMouseCursor.TextInput, Cursor.CursorType.Text);
            LoadMouseCursor(ImGuiMouseCursor.ResizeAll, Cursor.CursorType.SizeAll);
            LoadMouseCursor(ImGuiMouseCursor.ResizeNS, Cursor.CursorType.SizeVertical);
            LoadMouseCursor(ImGuiMouseCursor.ResizeEW, Cursor.CursorType.SizeHorinzontal);
            LoadMouseCursor(ImGuiMouseCursor.ResizeNESW,
                Cursor.CursorType.SizeBottomLeftTopRight);
            LoadMouseCursor(ImGuiMouseCursor.ResizeNWSE,
                Cursor.CursorType.SizeTopLeftBottomRight);
            LoadMouseCursor(ImGuiMouseCursor.Hand, Cursor.CursorType.Hand);

            if (loadDefaultFont)
            {
                // this will load default font automatically
                // No need to call AddDefaultFont
                UpdateFontTexture();
            }

            _windowHasFocus = window.HasFocus();

            window.MouseButtonPressed += OnMouseButtonPressed;
            window.MouseButtonReleased += OnMouseButtonReleased;
            window.MouseWheelScrolled += OnMouseWheelScrolled;
            window.KeyPressed += OnKeyPressed;
            window.KeyReleased += OnKeyReleased;
            window.TextEntered += OnTextEntered;
            window.GainedFocus += OnGainedFocus;
            window.LostFocus += OnLostFocus;

            
            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            NativeWindowSettings nativeWindow = new NativeWindowSettings();
            nativeWindow.StartVisible = false;

            nativeWindow.APIVersion = System.Version.Parse("3.1");
            nativeWindow.Profile = OpenTK.Windowing.Common.ContextProfile.Any;

            GameWindow gameWindow = new GameWindow(gameWindowSettings, nativeWindow);
            gameWindow.IsVisible = false;

        }

        private static void OnMouseButtonPressed(object sender, MouseButtonEventArgs args)
        {
            var button = (int)args.Button;
            if (button >= 0 && button < 3)
            {
                MousePressed[(int)args.Button] = true;
            }
        }

        private static void OnMouseButtonReleased(object sender, MouseButtonEventArgs args)
        {
            var button = (int)args.Button;
            if (button >= 0 && button < 3)
            {
                MousePressed[(int)args.Button] = false;
            }
        }

        private static void OnMouseWheelScrolled(object sender, MouseWheelScrollEventArgs args)
        {
            var io = ImGui.GetIO();
            switch (args.Wheel)
            {
                case Mouse.Wheel.VerticalWheel:
                case Mouse.Wheel.HorizontalWheel when io.KeyShift:
                    io.MouseWheel += args.Delta;
                    break;
                case Mouse.Wheel.HorizontalWheel:
                    io.MouseWheelH += args.Delta;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void OnKeyPressed(object sender, KeyEventArgs args)
        {
            var io = ImGui.GetIO();
            io.KeysDown[(int)args.Code] = true;
        }

        private static void OnKeyReleased(object sender, KeyEventArgs args)
        {
            var io = ImGui.GetIO();
            io.KeysDown[(int)args.Code] = false;
        }

        private static void OnTextEntered(object sender, TextEventArgs args)
        {
            var io = ImGui.GetIO();
            // Don't handle the event for unprintable characters
            if (args.Unicode[0] < ' ' || args.Unicode[0] == 127)
            {
                return;
            }

            io.AddInputCharacter(args.Unicode[0]);
        }

        private static void OnGainedFocus(object sender, EventArgs args)
        {
            _windowHasFocus = true;
        }

        private static void OnLostFocus(object sender, EventArgs args)
        {
            _windowHasFocus = false;
        }

        public static void Update(RenderWindow window, Time dt)
        {
            Update(window, window, dt);
        }

        public static void Update(Window window, RenderTarget target, Time dt)
        {
            // Update OS/hardware mouse cursor if imgui isn't drawing a software cursor
            UpdateMouseCursor(window);

            Update(Mouse.GetPosition(window), new Vector2f(target.Size.X, target.Size.Y), dt);

            if (ImGui.GetIO().MouseDrawCursor)
            {
                // Hide OS mouse cursor if imgui is drawing it
                window.SetMouseCursorVisible(false);
            }
        }

        public static void Update(Vector2i mousePos, Vector2f displaySize, Time dt)
        {
            var io = ImGui.GetIO();
            io.DisplaySize = new Vector2(displaySize.X, displaySize.Y);

            io.DeltaTime = dt.AsSeconds();

            if (_windowHasFocus)
            {
                if (io.WantSetMousePos)
                {
                    var newMousePos = new Vector2i((int)io.MousePos.X, (int)io.MousePos.Y);
                    Mouse.SetPosition(newMousePos);
                }
                else
                {
                    io.MousePos = new Vector2(mousePos.X, mousePos.Y);
                }

                for (var i = 0; i < 3; i++)
                {
                    io.MouseDown[i] = TouchDown[i] || Touch.IsDown((uint)i) ||
                                      MousePressed[i] ||
                                      Mouse.IsButtonPressed((Mouse.Button)i);
                    MousePressed[i] = false;
                    TouchDown[i] = false;
                }
            }

            // Update Ctrl, Shift, Alt, Super state
            io.KeyCtrl = io.KeysDown[(int)Keyboard.Key.LControl] ||
                         io.KeysDown[(int)Keyboard.Key.RControl];
            io.KeyAlt =
                io.KeysDown[(int)Keyboard.Key.LAlt] || io.KeysDown[(int)Keyboard.Key.RAlt];
            io.KeyShift =
                io.KeysDown[(int)Keyboard.Key.LShift] || io.KeysDown[(int)Keyboard.Key.RShift];
            io.KeySuper = io.KeysDown[(int)Keyboard.Key.LSystem] ||
                          io.KeysDown[(int)Keyboard.Key.RSystem];

            //assert(io.Fonts->Fonts.Size > 0);  // You forgot to create and set up font
            //                                   // atlas (see createFontTexture)

            ImGui.NewFrame();
        }

        public static void Render(RenderTarget target)
        {
            target.ResetGLStates();
            ImGui.Render();
            RenderDrawLists(ImGui.GetDrawData(), ImGui.GetIO());
        }

        public static void Render()
        {
            ImGui.Render();
            RenderDrawLists(ImGui.GetDrawData(), ImGui.GetIO());
        }

        private unsafe static void RenderDrawLists(ImDrawDataPtr drawData, ImGuiIOPtr io)
        {
            if (drawData.CmdListsCount == 0)
            {
                return;
            }

            int fb_width = (int)(io.DisplaySize.X * io.DisplayFramebufferScale.X);
            int fb_height = (int)(io.DisplaySize.Y * io.DisplayFramebufferScale.Y);

            if (fb_width == 0 || fb_height == 0)
            {
                return;
            }

            drawData.ScaleClipRects(io.DisplayFramebufferScale);

            //int lastTexutre = GL.GetInteger(GetPName.TextureBinding2D);
            //int lastArrayBuffer = GL.GetInteger(GetPName.ArrayBufferBinding);
            //int lastElementArrayBuffer = GL.GetInteger(GetPName.ElementArrayBufferBinding);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ScissorTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);
            OpenTK.Graphics.ES11.GL.EnableClientState(OpenTK.Graphics.ES11.EnableCap.VertexArray);
            OpenTK.Graphics.ES11.GL.EnableClientState(OpenTK.Graphics.ES11.EnableCap.ColorArray);
            OpenTK.Graphics.ES11.GL.EnableClientState(OpenTK.Graphics.ES11.EnableCap.TextureCoordArray);

            GL.Viewport(0, 0, fb_width, fb_height);

            OpenTK.Graphics.ES11.GL.MatrixMode(OpenTK.Graphics.ES11.MatrixMode.Texture);
            OpenTK.Graphics.ES11.GL.LoadIdentity();

            OpenTK.Graphics.ES11.GL.MatrixMode(OpenTK.Graphics.ES11.MatrixMode.Projection);
            OpenTK.Graphics.ES11.GL.LoadIdentity();

            OpenTK.Graphics.ES11.GL.Ortho(0f, io.DisplaySize.X, io.DisplaySize.Y, 0f, -1f, 1f);

            OpenTK.Graphics.ES11.GL.MatrixMode(OpenTK.Graphics.ES11.MatrixMode.Modelview);
            OpenTK.Graphics.ES11.GL.LoadIdentity();
            //System.Console.WriteLine("-----------------------------------");
            //System.Console.WriteLine(drawData.CmdListsCount);
            for (int n = 0; n < drawData.CmdListsCount; n++)
            {
                ImDrawListPtr cmdList = drawData.CmdListsRange[n];
                OpenTK.Graphics.ES11.GL.VertexPointer(2, OpenTK.Graphics.ES11.VertexPointerType.Float, Unsafe.SizeOf<ImDrawVert>(), cmdList.VtxBuffer.Data + 0);
                OpenTK.Graphics.ES11.GL.TexCoordPointer(2, OpenTK.Graphics.ES11.TexCoordPointerType.Float, Unsafe.SizeOf<ImDrawVert>(), cmdList.VtxBuffer.Data + 8);
                OpenTK.Graphics.ES11.GL.ColorPointer(4, OpenTK.Graphics.ES11.ColorPointerType.UnsignedByte, Unsafe.SizeOf<ImDrawVert>(), cmdList.VtxBuffer.Data + 16);

                var idx_buffer = cmdList.IdxBuffer.Data;

                for (int cmdIndex = 0; cmdIndex < cmdList.CmdBuffer.Size; cmdIndex++)
                {
                    ImDrawCmdPtr drawCommand = cmdList.CmdBuffer[cmdIndex];
                    //System.Console.WriteLine(cmdList.CmdBuffer.Size);
                    if (drawCommand.UserCallback != IntPtr.Zero)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        var textureHandle = drawCommand.TextureId;
                        GL.BindTexture(TextureTarget.Texture2D, (int)textureHandle);
                        GL.Scissor((int)drawCommand.ClipRect.X, (int)(fb_height - drawCommand.ClipRect.W), (int)(drawCommand.ClipRect.Z - drawCommand.ClipRect.X), (int)(drawCommand.ClipRect.W - drawCommand.ClipRect.Y));
                        GL.DrawElements(OpenTK.Graphics.ES20.PrimitiveType.Triangles, (int)drawCommand.ElemCount, DrawElementsType.UnsignedShort, idx_buffer + (int)drawCommand.IdxOffset);
                    }
                    idx_buffer += (Int32)drawCommand.ElemCount;
                }
            }

            //GL.BindTexture(TextureTarget.Texture2D,lastTexutre);
            //GL.BindBuffer(BufferTarget.ArrayBuffer,lastArrayBuffer);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer,lastElementArrayBuffer);
            GL.Disable(EnableCap.ScissorTest);
        }

        private static unsafe IntPtr ConvertGlTextureHandleToImTextureId(uint glTextureHandle)
        {
            var imTexId = 0;
            long size = Unsafe.SizeOf<uint>();
            System.Buffer.MemoryCopy(&glTextureHandle, &imTexId, size, size);
            return new IntPtr(imTexId);
        }

        private static unsafe uint ConvertImTextureIdToGlTextureHandle(IntPtr imTexId)
        {
            uint glTextureHandle = 0;
            long size = Unsafe.SizeOf<uint>();
            System.Buffer.MemoryCopy(imTexId.ToPointer(), new IntPtr(&glTextureHandle).ToPointer(), size, size);
            return glTextureHandle;
        }

        public static void Shutdown()
        {
            ImGui.GetIO().Fonts.TexID = IntPtr.Zero;
            _fontTexture.Dispose();
            for (var i = 0; i < (int)ImGuiMouseCursor.COUNT; ++i)
            {
                if (MouseCursors[i] != null)
                {
                    MouseCursors[i].Dispose();
                }
            }

            ImGui.DestroyContext();
        }

        public static void UpdateFontTexture()
        {
            var io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out var width, out var height, out var bpp);

            _fontTexture = new Texture((uint)width, (uint)height);

            var size = width * height * bpp;
            var byteArray = new byte[size];
            Marshal.Copy(pixels, byteArray, 0, size);

            _fontTexture.Update(byteArray);

            io.Fonts.SetTexID(ConvertGlTextureHandleToImTextureId(_fontTexture.NativeHandle));
        }

        public static Texture GetFontTexture()
        {
            return _fontTexture;
        }

        private static string ClipboadText
        {
            get => Clipboard.Contents;
            set => Clipboard.Contents = value;
        }

        private static void LoadMouseCursor(ImGuiMouseCursor imguiCursorType, Cursor.CursorType sfmlCursorType)
        {
            MouseCursors[(int)imguiCursorType] = new Cursor(sfmlCursorType);
            System.Console.WriteLine(MouseCursors[(int)imguiCursorType].CPointer);
        }

        private static void UpdateMouseCursor(Window window)
        {
            var io = ImGui.GetIO();
            if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0) return;

            var cursor = ImGui.GetMouseCursor();
            if (io.MouseDrawCursor || cursor == ImGuiMouseCursor.None)
            {
                window.SetMouseCursorVisible(false);
            }
            else
            {
                window.SetMouseCursorVisible(true);

                var sfmlCursor = MouseCursors[(int)cursor] ?? MouseCursors[(int)ImGuiMouseCursor.Arrow];
                if (sfmlCursor.CPointer != IntPtr.Zero)
                {
                    window.SetMouseCursor(sfmlCursor);
                }

            }
        }

        private static void Image(Texture texture, Color tintColor, Color borderColor)
        {
            Image(texture, new Vector2f(texture.Size.X, texture.Size.Y), tintColor, borderColor);
        }

        private static void Image(Texture texture, Vector2f size, Color tintColor, Color borderColor)
        {
            var textureId = ConvertGlTextureHandleToImTextureId(texture.NativeHandle);
            ImGui.Image(textureId, new Vector2(size.X, size.Y), new Vector2(0, 0), new Vector2(1, 1),
                ToImColor(tintColor).Value, ToImColor(borderColor).Value);
        }

        private static void Image(Texture texture, FloatRect textureRect, Color tintColor, Color borderColor)
        {
            Image(texture, new Vector2f(Math.Abs(textureRect.Width), Math.Abs(textureRect.Height)),
                textureRect, tintColor, borderColor);
        }

        private static void Image(Texture texture, Vector2f size, FloatRect textureRect, Color tintColor,
            Color borderColor)
        {
            var textureSize = texture.Size;
            var uv0 = new Vector2(textureRect.Left / textureSize.X, textureRect.Top / textureSize.Y);

            var uv1 = new Vector2((textureRect.Left + textureRect.Width) / textureSize.X,
                (textureRect.Top + textureRect.Height) / textureSize.Y);

            var textureId = ConvertGlTextureHandleToImTextureId(texture.NativeHandle);
            ImGui.Image(textureId, new Vector2(size.X, size.Y), uv0, uv1,
                ToImColor(tintColor).Value, ToImColor(borderColor).Value);
        }

        private static void Image(Sprite sprite, Color tintColor, Color borderColor)
        {
            var bounds = sprite.GetGlobalBounds();
            Image(sprite, new Vector2f(bounds.Width, bounds.Height), tintColor, borderColor);
        }

        private static void Image(Sprite sprite, Vector2f size, Color tintColor, Color borderColor)
        {
            var texture = sprite.Texture;
            // sprite without texture cannot be drawn
            if (texture == null)
            {
                return;
            }

            var tr = sprite.TextureRect;
            Image(texture, size, new FloatRect(tr.Left, tr.Top, tr.Width, tr.Height), tintColor, borderColor);
        }

        // ImageButton overloads
        private static bool ImageButton(Texture texture, int framePadding, Color bgColor, Color tintColor)
        {
            return ImageButton(texture, new Vector2f(texture.Size.X, texture.Size.Y), framePadding, bgColor, tintColor);
        }

        private static bool ImageButton(Texture texture, Vector2f size, int framePadding, Color bgColor,
            Color tintColor)
        {
            var textureSize = texture.Size;
            return ImageButtonImpl(texture, new FloatRect(0, 0, textureSize.X, textureSize.Y),
                size, framePadding, bgColor, tintColor);
        }

        private static bool ImageButton(Sprite sprite, int framePadding, Color bgColor, Color tintColor)
        {
            var spriteSize = sprite.GetGlobalBounds();
            return ImageButton(sprite, new Vector2f(spriteSize.Width, spriteSize.Height),
                framePadding, bgColor, tintColor);
        }

        private static bool ImageButton(Sprite sprite, Vector2f size, int framePadding, Color bgColor, Color tintColor)
        {
            var texture = sprite.Texture;
            if (texture == null)
            {
                return false;
            }

            var tr = sprite.TextureRect;

            return ImageButtonImpl(texture, new FloatRect(tr.Left, tr.Top, tr.Width, tr.Height), size,
                framePadding, bgColor, tintColor);
        }

        // Draw_list overloads. All positions are in relative coordinates (relative to top-left of the current window)
        private static void DrawLine(Vector2f a, Vector2f b, Color col, float thickness = 1.0f)
        {
            var drawList = ImGui.GetWindowDrawList();
            var pos = ImGui.GetCursorScreenPos();
            drawList.AddLine(new Vector2(a.X + pos.X, a.Y + pos.Y), new Vector2(b.X + pos.X, b.Y + pos.Y),
                ColorConvertFloat4ToU32(ToImColor(col).Value), thickness);
        }

        private static void DrawRect(FloatRect rect, Color color, float rounding = 0.0f, int roundingCorners =
            0x0F, float thickness = 1.0f)
        {
            var drawList = ImGui.GetWindowDrawList();
            drawList.AddRect(GetTopLeftAbsolute(rect), GetDownRightAbsolute(rect),
                ColorConvertFloat4ToU32(ToImColor(color).Value), rounding, (ImDrawCornerFlags)roundingCorners,
                thickness);
        }

        private static void DrawRectFilled(FloatRect rect, Color color, float rounding = 0.0f,
            int roundingCorners = 0x0F)
        {
            var drawList = ImGui.GetWindowDrawList();
            drawList.AddRectFilled(GetTopLeftAbsolute(rect), GetDownRightAbsolute(rect),
                ColorConvertFloat4ToU32(ToImColor(color).Value), rounding, (ImDrawCornerFlags)roundingCorners);
        }

        private static ImColor ToImColor(Color c)
        {
            return new ImColor { Value = new Vector4(c.R, c.G, c.B, c.A) };
        }

        private static unsafe uint ColorConvertFloat4ToU32(Vector4 c)
        {
            var src = new[] { (byte)c.X, (byte)c.Y, (byte)c.Z, (byte)c.W };
            return BitConverter.ToUInt32(src);
        }

        private static Vector2 GetTopLeftAbsolute(FloatRect rect)
        {
            var pos = ImGui.GetCursorScreenPos();
            return new Vector2(rect.Left + pos.X, rect.Top + pos.Y);
        }

        private static Vector2 GetDownRightAbsolute(FloatRect rect)
        {
            var pos = ImGui.GetCursorScreenPos();
            return new Vector2(rect.Left + rect.Width + pos.X, rect.Top + rect.Height + pos.Y);
        }

        private static bool ImageButtonImpl(Texture texture, FloatRect textureRect, Vector2f size, int framePadding,
            Color bgColor, Color tintColor)
        {
            var textureSize = texture.Size;

            var uv0 = new Vector2(textureRect.Left / textureSize.X, textureRect.Top / textureSize.Y);
            var uv1 = new Vector2((textureRect.Left + textureRect.Width) / textureSize.X,
                (textureRect.Top + textureRect.Height) / textureSize.Y);

            var textureId = ConvertGlTextureHandleToImTextureId(texture.NativeHandle);
            return ImGui.ImageButton(textureId, new Vector2(size.X, size.Y), uv0, uv1, framePadding,
                ToImColor(bgColor).Value, ToImColor(tintColor).Value);
        }
    }
}

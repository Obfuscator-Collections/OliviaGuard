﻿using dnlib.DotNet;
using dnlib.DotNet.Emit;
using OliviaGuard.Protector.Class;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OliviaGuard.Protector.Protections
{
	public static class Utils
	{
		public static Random rnd = new Random();
		public static byte[] RandomByteArr(int size)
		{
			var result = new byte[size];
			rnd.NextBytes(result);
			return result;
		}

		public static Code GetCode(bool supported = false)
		{
			var codes = new Code[] { Code.Add, Code.And, Code.Xor, Code.Sub, Code.Or };
			if (supported)
				codes = new Code[] { Code.Add, Code.Sub, Code.Xor };
			return codes[rnd.Next(0, codes.Length)];
		}

		public static FieldDefUser CreateField(FieldSig sig)
		{
			return new FieldDefUser(GenerateString(), sig, FieldAttributes.Public | FieldAttributes.Static);
		}

		public static MethodDefUser CreateMethod(ModuleDef mod)
		{
			var method = new MethodDefUser(GenerateString(), MethodSig.CreateStatic(mod.CorLibTypes.Void),
				MethodImplAttributes.IL | MethodImplAttributes.Managed,
				MethodAttributes.Public | MethodAttributes.Static);
			method.Body = new CilBody();
			method.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
			mod.GlobalType.Methods.Add(method);
			return method;
		}

		public static int RandomSmallInt32() => rnd.Next(15, 40);
		public static int RandomInt32() => rnd.Next(100, 500);
		public static int RandomBigInt32() => rnd.Next();
		public static bool RandomBoolean() => Convert.ToBoolean(rnd.Next(0, 2));
		public static string GenerateString()
		{
			int seed = rnd.Next();
			return (seed * 0x19660D + 0x3C6EF35).ToString("X");
		}

		public static string GetRealString()
		{
			string[] charsc = { "CausalityTraceLevel", "BitConverter", "UnhandledExceptionEventHandler", "PinnedBufferMemoryStream", "RichTextBoxScrollBars", "RichTextBoxSelectionAttribute", "RichTextBoxSelectionTypes", "RichTextBoxStreamType", "RichTextBoxWordPunctuations", "RightToLeft", "RTLAwareMessageBox", "SafeNativeMethods", "SaveFileDialog", "Screen", "ScreenOrientation", "ScrollableControl", "ScrollBar", "ScrollBarRenderer", "ScrollBars", "ScrollButton", "ScrollEventArgs", "ScrollEventHandler", "ScrollEventType", "ScrollOrientation", "ScrollProperties", "SearchDirectionHint", "SearchForVirtualItemEventArgs", "SearchForVirtualItemEventHandler", "SecurityIDType", "SelectedGridItemChangedEventArgs", "SelectedGridItemChangedEventHandler", "SelectionMode", "SelectionRange", "SelectionRangeConverter", "SendKeys", "Shortcut", "SizeGripStyle", "SortOrder", "SpecialFolderEnumConverter", "SplitContainer", "Splitter", "SplitterCancelEventArgs", "SplitterCancelEventHandler", "SplitterEventArgs", "SplitterEventHandler", "SplitterPanel", "StatusBar", "StatusBarDrawItemEventArgs", "StatusBarDrawItemEventHandler", "StatusBarPanel", "StatusBarPanelAutoSize", "StatusBarPanelBorderStyle", "StatusBarPanelClickEventArgs", "StatusBarPanelClickEventHandler", "StatusBarPanelStyle", "StatusStrip", "StringSorter", "StringSource", "StructFormat", "SystemInformation", "SystemParameter", "TabAlignment", "TabAppearance", "TabControl", "TabControlAction", "TabControlCancelEventArgs", "TabControlCancelEventHandler", "TabControlEventArgs", "TabControlEventHandler", "TabDrawMode", "TableLayoutPanel", "TableLayoutControlCollection", "TableLayoutPanelCellBorderStyle", "TableLayoutPanelCellPosition", "TableLayoutPanelCellPositionTypeConverter", "TableLayoutPanelGrowStyle", "TableLayoutSettings", "SizeType", "ColumnStyle", "RowStyle", "TableLayoutStyle", "TableLayoutStyleCollection", "TableLayoutCellPaintEventArgs", "TableLayoutCellPaintEventHandler", "TableLayoutColumnStyleCollection", "TableLayoutRowStyleCollection", "TabPage", "TabRenderer", "TabSizeMode", "TextBox", "TextBoxAutoCompleteSourceConverter", "TextBoxBase", "TextBoxRenderer", "TextDataFormat", "TextImageRelation", "ThreadExceptionDialog", "TickStyle", "ToolBar", "ToolBarAppearance", "ToolBarButton", "ToolBarButtonClickEventArgs", "ToolBarButtonClickEventHandler", "ToolBarButtonStyle", "ToolBarTextAlign", "ToolStrip", "CachedItemHdcInfo", "MouseHoverTimer", "ToolStripSplitStackDragDropHandler", "ToolStripArrowRenderEventArgs", "ToolStripArrowRenderEventHandler", "ToolStripButton", "ToolStripComboBox", "ToolStripControlHost", "ToolStripDropDown", "ToolStripDropDownCloseReason", "ToolStripDropDownClosedEventArgs", "ToolStripDropDownClosedEventHandler", "ToolStripDropDownClosingEventArgs", "ToolStripDropDownClosingEventHandler", "ToolStripDropDownDirection", "ToolStripDropDownButton", "ToolStripDropDownItem", "ToolStripDropDownItemAccessibleObject", "ToolStripDropDownMenu", "ToolStripDropTargetManager", "ToolStripHighContrastRenderer", "ToolStripGrip", "ToolStripGripDisplayStyle", "ToolStripGripRenderEventArgs", "ToolStripGripRenderEventHandler", "ToolStripGripStyle", "ToolStripItem", "ToolStripItemImageIndexer", "ToolStripItemInternalLayout", "ToolStripItemAlignment", "ToolStripItemClickedEventArgs", "ToolStripItemClickedEventHandler", "ToolStripItemCollection", "ToolStripItemDisplayStyle", "ToolStripItemEventArgs", "ToolStripItemEventHandler", "ToolStripItemEventType", "ToolStripItemImageRenderEventArgs", "ToolStripItemImageRenderEventHandler", "ToolStripItemImageScaling", "ToolStripItemOverflow", "ToolStripItemPlacement", "ToolStripItemRenderEventArgs", "ToolStripItemRenderEventHandler", "ToolStripItemStates", "ToolStripItemTextRenderEventArgs", "ToolStripItemTextRenderEventHandler", "ToolStripLabel", "ToolStripLayoutStyle", "ToolStripManager", "ToolStripCustomIComparer", "MergeHistory", "MergeHistoryItem", "ToolStripManagerRenderMode", "ToolStripMenuItem", "MenuTimer", "ToolStripMenuItemInternalLayout", "ToolStripOverflow", "ToolStripOverflowButton", "ToolStripContainer", "ToolStripContentPanel", "ToolStripPanel", "ToolStripPanelCell", "ToolStripPanelRenderEventArgs", "ToolStripPanelRenderEventHandler", "ToolStripContentPanelRenderEventArgs", "ToolStripContentPanelRenderEventHandler", "ToolStripPanelRow", "ToolStripPointType", "ToolStripProfessionalRenderer", "ToolStripProfessionalLowResolutionRenderer", "ToolStripProgressBar", "ToolStripRenderer", "ToolStripRendererSwitcher", "ToolStripRenderEventArgs", "ToolStripRenderEventHandler", "ToolStripRenderMode", "ToolStripScrollButton", "ToolStripSeparator", "ToolStripSeparatorRenderEventArgs", "ToolStripSeparatorRenderEventHandler", "ToolStripSettings", "ToolStripSettingsManager", "ToolStripSplitButton", "ToolStripSplitStackLayout", "ToolStripStatusLabel", "ToolStripStatusLabelBorderSides", "ToolStripSystemRenderer", "ToolStripTextBox", "ToolStripTextDirection", "ToolStripLocationCancelEventArgs", "ToolStripLocationCancelEventHandler", "ToolTip", "ToolTipIcon", "TrackBar", "TrackBarRenderer", "TreeNode", "TreeNodeMouseClickEventArgs", "TreeNodeMouseClickEventHandler", "TreeNodeCollection", "TreeNodeConverter", "TreeNodeMouseHoverEventArgs", "TreeNodeMouseHoverEventHandler", "TreeNodeStates", "TreeView", "TreeViewAction", "TreeViewCancelEventArgs", "TreeViewCancelEventHandler", "TreeViewDrawMode", "TreeViewEventArgs", "TreeViewEventHandler", "TreeViewHitTestInfo", "TreeViewHitTestLocations", "TreeViewImageIndexConverter", "TreeViewImageKeyConverter", "Triangle", "TriangleDirection", "TypeValidationEventArgs", "TypeValidationEventHandler", "UICues", "UICuesEventArgs", "UICuesEventHandler", "UpDownBase", "UpDownEventArgs", "UpDownEventHandler", "UserControl", "ValidationConstraints", "View", "VScrollBar", "VScrollProperties", "WebBrowser", "WebBrowserEncryptionLevel", "WebBrowserReadyState", "WebBrowserRefreshOption", "WebBrowserBase", "WebBrowserContainer", "WebBrowserDocumentCompletedEventHandler", "WebBrowserDocumentCompletedEventArgs", "WebBrowserHelper", "WebBrowserNavigatedEventHandler", "WebBrowserNavigatedEventArgs", "WebBrowserNavigatingEventHandler", "WebBrowserNavigatingEventArgs", "WebBrowserProgressChangedEventHandler", "WebBrowserProgressChangedEventArgs", "WebBrowserSiteBase", "WebBrowserUriTypeConverter", "WinCategoryAttribute", "WindowsFormsSection", "WindowsFormsSynchronizationContext", "IntSecurity", "WindowsFormsUtils", "IComponentEditorPageSite", "LayoutSettings", "PageSetupDialog", "PrintControllerWithStatusDialog", "PrintDialog", "PrintPreviewControl", "PrintPreviewDialog", "TextFormatFlags", "TextRenderer", "WindowsGraphicsWrapper", "SRDescriptionAttribute", "SRCategoryAttribute", "SR", "VisualStyleElement", "VisualStyleInformation", "VisualStyleRenderer", "VisualStyleState", "ComboBoxState", "CheckBoxState", "GroupBoxState", "HeaderItemState", "PushButtonState", "RadioButtonState", "ScrollBarArrowButtonState", "ScrollBarState", "ScrollBarSizeBoxState", "TabItemState", "TextBoxState", "ToolBarState", "TrackBarThumbState", "BackgroundType", "BorderType", "ImageOrientation", "SizingType", "FillType", "HorizontalAlign", "ContentAlignment", "VerticalAlignment", "OffsetType", "IconEffect", "TextShadowType", "GlyphType", "ImageSelectType", "TrueSizeScalingType", "GlyphFontSizingType", "ColorProperty", "EnumProperty", "FilenameProperty", "FontProperty", "IntegerProperty", "PointProperty", "MarginProperty", "StringProperty", "BooleanProperty", "Edges", "EdgeStyle", "EdgeEffects", "TextMetrics", "TextMetricsPitchAndFamilyValues", "TextMetricsCharacterSet", "HitTestOptions", "HitTestCode", "ThemeSizeType", "VisualStyleDocProperty", "VisualStyleSystemProperty", "ArrayElementGridEntry", "CategoryGridEntry", "DocComment", "DropDownButton", "DropDownButtonAdapter", "GridEntry", "AttributeTypeSorter", "GridEntryRecreateChildrenEventHandler", "GridEntryRecreateChildrenEventArgs", "GridEntryCollection", "GridErrorDlg", "GridToolTip", "HotCommands", "ImmutablePropertyDescriptorGridEntry", "IRootGridEntry", "MergePropertyDescriptor", "MultiPropertyDescriptorGridEntry", "MultiSelectRootGridEntry", "PropertiesTab", "PropertyDescriptorGridEntry", "PropertyGridCommands", "PropertyGridView", "SingleSelectRootGridEntry", "ComponentEditorForm", "ComponentEditorPage", "EventsTab", "IUIService", "IWindowsFormsEditorService", "PropertyTab", "ToolStripItemDesignerAvailability", "ToolStripItemDesignerAvailabilityAttribute", "WindowsFormsComponentEditor", "BaseCAMarshaler", "Com2AboutBoxPropertyDescriptor", "Com2ColorConverter", "Com2ComponentEditor", "Com2DataTypeToManagedDataTypeConverter", "Com2Enum", "Com2EnumConverter", "Com2ExtendedBrowsingHandler", "Com2ExtendedTypeConverter", "Com2FontConverter", "Com2ICategorizePropertiesHandler", "Com2IDispatchConverter", "Com2IManagedPerPropertyBrowsingHandler", "Com2IPerPropertyBrowsingHandler", "Com2IProvidePropertyBuilderHandler", "Com2IVsPerPropertyBrowsingHandler", "Com2PictureConverter", "Com2Properties", "Com2PropertyBuilderUITypeEditor", "Com2PropertyDescriptor", "GetAttributesEvent", "Com2EventHandler", "GetAttributesEventHandler", "GetNameItemEvent", "GetNameItemEventHandler", "DynamicMetaObjectProviderDebugView", "ExpressionTreeCallRewriter", "ICSharpInvokeOrInvokeMemberBinder", "ResetBindException", "RuntimeBinder", "RuntimeBinderController", "RuntimeBinderException", "RuntimeBinderInternalCompilerException", "SpecialNames", "SymbolTable", "RuntimeBinderExtensions", "NameManager", "Name", "NameTable", "OperatorKind", "PredefinedName", "PredefinedType", "TokenFacts", "TokenKind", "OutputContext", "UNSAFESTATES", "CheckedContext", "BindingFlag", "ExpressionBinder", "BinOpKind", "BinOpMask", "CandidateFunctionMember", "ConstValKind", "CONSTVAL", "ConstValFactory", "ConvKind", "CONVERTTYPE", "BetterType", "ListExtensions", "CConversions", "Operators", "UdConvInfo", "ArgInfos", "BodyType", "ConstCastResult", "AggCastResult", "UnaryOperatorSignatureFindResult", "UnaOpKind", "UnaOpMask", "OpSigFlags", "LiftFlags", "CheckLvalueKind", "BinOpFuncKind", "UnaOpFuncKind", "ExpressionKind", "ExpressionKindExtensions", "EXPRExtensions", "ExprFactory", "EXPRFLAG", "FileRecord", "FUNDTYPE", "GlobalSymbolContext", "InputFile", "LangCompiler", "MemLookFlags", "MemberLookup", "CMemberLookupResults", "mdToken", "CorAttributeTargets", "MethodKindEnum", "MethodTypeInferrer", "NameGenerator", "CNullable", "NullableCallLiftKind", "CONSTRESKIND", "LambdaParams", "TypeOrSimpleNameResolution", "InitializerKind", "ConstantStringConcatenation", "ForeachKind", "PREDEFATTR", "PREDEFMETH", "PREDEFPROP", "MethodRequiredEnum", "MethodCallingConventionEnum", "MethodSignatureEnum", "PredefinedMethodInfo", "PredefinedPropertyInfo", "PredefinedMembers", "ACCESSERROR", "CSemanticChecker", "SubstTypeFlags", "SubstContext", "CheckConstraintsFlags", "TypeBind", "UtilityTypeExtensions", "SymWithType", "MethPropWithType", "MethWithType", "PropWithType", "EventWithType", "FieldWithType", "MethPropWithInst", "MethWithInst", "AggregateDeclaration", "Declaration", "GlobalAttributeDeclaration", "ITypeOrNamespace", "AggregateSymbol", "AssemblyQualifiedNamespaceSymbol", "EventSymbol", "FieldSymbol", "IndexerSymbol", "LabelSymbol", "LocalVariableSymbol", "MethodOrPropertySymbol", "MethodSymbol", "InterfaceImplementationMethodSymbol", "IteratorFinallyMethodSymbol", "MiscSymFactory", "NamespaceOrAggregateSymbol", "NamespaceSymbol", "ParentSymbol", "PropertySymbol", "Scope", "KAID", "ACCESS", "AggKindEnum", "ARRAYMETHOD", "SpecCons", "Symbol", "SymbolExtensions", "SymFactory", "SymFactoryBase", "SYMKIND", "SynthAggKind", "SymbolLoader", "AidContainer", "BSYMMGR", "symbmask_t", "SYMTBL", "TransparentIdentifierMemberSymbol", "TypeParameterSymbol", "UnresolvedAggregateSymbol", "VariableSymbol", "EXPRARRAYINDEX", "EXPRARRINIT", "EXPRARRAYLENGTH", "EXPRASSIGNMENT", "EXPRBINOP", "EXPRBLOCK", "EXPRBOUNDLAMBDA", "EXPRCALL", "EXPRCAST", "EXPRCLASS", "EXPRMULTIGET", "EXPRMULTI", "EXPRCONCAT", "EXPRQUESTIONMARK", "EXPRCONSTANT", "EXPREVENT", "EXPR", "ExpressionIterator", "EXPRFIELD", "EXPRFIELDINFO", "EXPRHOISTEDLOCALEXPR", "EXPRLIST", "EXPRLOCAL", "EXPRMEMGRP", "EXPRMETHODINFO", "EXPRFUNCPTR", "EXPRNamedArgumentSpecification", "EXPRPROP", "EXPRPropertyInfo", "EXPRRETURN", "EXPRSTMT", "EXPRWRAP", "EXPRTHISPOINTER", "EXPRTYPEARGUMENTS", "EXPRTYPEOF", "EXPRTYPEORNAMESPACE", "EXPRUNARYOP", "EXPRUNBOUNDLAMBDA", "EXPRUSERDEFINEDCONVERSION", "EXPRUSERLOGOP", "EXPRZEROINIT", "ExpressionTreeRewriter", "ExprVisitorBase", "AggregateType", "ArgumentListType", "ArrayType", "BoundLambdaType", "ErrorType", "MethodGroupType", "NullableType", "NullType", "OpenTypePlaceholderType", "ParameterModifierType", "PointerType", "PredefinedTypes", "PredefinedTypeFacts", "CType", "TypeArray", "TypeFactory", "TypeManager", "TypeParameterType", "KeyPair`2", "TypeTable", "VoidType", "CError", "CParameterizedError", "CErrorFactory", "ErrorFacts", "ErrArgKind", "ErrArgFlags", "SymWithTypeMemo", "MethPropWithInstMemo", "ErrArg", "ErrArgRef", "ErrArgRefOnly", "ErrArgNoRef", "ErrArgIds", "ErrArgSymKind", "ErrorHandling", "IErrorSink", "MessageID", "UserStringBuilder", "CController", "<Cons>d__10`1", "<Cons>d__11`1", "DynamicProperty", "DynamicDebugViewEmptyException", "<>c__DisplayClass20_0", "ExpressionEXPR", "ArgumentObject", "NameHashKey", "<>c__DisplayClass18_0", "<>c__DisplayClass18_1", "<>c__DisplayClass43_0", "<>c__DisplayClass45_0", "KnownName", "BinOpArgInfo", "BinOpSig", "BinOpFullSig", "ConversionFunc", "ExplicitConversion", "PfnBindBinOp", "PfnBindUnaOp", "GroupToArgsBinder", "GroupToArgsBinderResult", "ImplicitConversion", "UnaOpSig", "UnaOpFullSig", "OPINFO", "<ToEnumerable>d__1", "CMethodIterator", "NewInferenceResult", "Dependency", "<InterfaceAndBases>d__0", "<AllConstraintInterfaces>d__1", "<TypeAndBaseClasses>d__2", "<TypeAndBaseClassInterfaces>d__3", "<AllPossibleInterfaces>d__4", "<Children>d__0", "Kind", "TypeArrayKey", "Key", "PredefinedTypeInfo", "StdTypeVarColl", "<>c__DisplayClass71_0", "__StaticArrayInitTypeSize=104", "__StaticArrayInitTypeSize=169", "SNINativeMethodWrapper", "QTypes", "ProviderEnum", "IOType", "ConsumerNumber", "SqlAsyncCallbackDelegate", "ConsumerInfo", "SNI_Error", "Win32NativeMethods", "NativeOledbWrapper", "AdalException", "ADALNativeWrapper", "Sni_Consumer_Info", "SNI_ConnWrapper", "SNI_Packet_IOType", "ConsumerNum", "$ArrayType$$$BY08$$CBG", "_GUID", "SNI_CLIENT_CONSUMER_INFO", "IUnknown", "__s_GUID", "IChapteredRowset", "_FILETIME", "ProviderNum", "ITransactionLocal", "SNI_ERROR", "$ArrayType$$$BY08G", "BOID", "ModuleLoadException", "ModuleLoadExceptionHandlerException", "ModuleUninitializer", "LanguageSupport", "gcroot<System::String ^>", "$ArrayType$$$BY00Q6MPBXXZ", "Progress", "$ArrayType$$$BY0A@P6AXXZ", "$ArrayType$$$BY0A@P6AHXZ", "__enative_startup_state", "TriBool", "ICLRRuntimeHost", "ThisModule", "_EXCEPTION_POINTERS", "Bid", "SqlDependencyProcessDispatcher", "BidIdentityAttribute", "BidMetaTextAttribute", "BidMethodAttribute", "BidArgumentTypeAttribute", "ExtendedClrTypeCode", "ITypedGetters", "ITypedGettersV3", "ITypedSetters", "ITypedSettersV3", "MetaDataUtilsSmi", "SmiConnection", "SmiContext", "SmiContextFactory", "SmiEventSink", "SmiEventSink_Default", "SmiEventSink_DeferedProcessing", "SmiEventStream", "SmiExecuteType", "SmiGettersStream", "SmiLink", "SmiMetaData", "SmiExtendedMetaData", "SmiParameterMetaData", "SmiStorageMetaData", "SmiQueryMetaData", "SmiRecordBuffer", "SmiRequestExecutor", "SmiSettersStream", "SmiStream", "SmiXetterAccessMap", "SmiXetterTypeCode", "SqlContext", "SqlDataRecord", "SqlPipe", "SqlTriggerContext", "ValueUtilsSmi", "SqlClientWrapperSmiStream", "SqlClientWrapperSmiStreamChars", "IBinarySerialize", "InvalidUdtException", "SqlFacetAttribute", "DataAccessKind", "SystemDataAccessKind", "SqlFunctionAttribute", "SqlMetaData", "SqlMethodAttribute", "FieldInfoEx", "BinaryOrderedUdtNormalizer", "Normalizer", "BooleanNormalizer", "SByteNormalizer", "ByteNormalizer", "ShortNormalizer", "UShortNormalizer", "IntNormalizer", "UIntNormalizer", "LongNormalizer", "ULongNormalizer", "FloatNormalizer", "DoubleNormalizer", "SqlProcedureAttribute", "SerializationHelperSql9", "Serializer", "NormalizedSerializer", "BinarySerializeSerializer", "DummyStream", "SqlTriggerAttribute", "SqlUserDefinedAggregateAttribute", "SqlUserDefinedTypeAttribute", "TriggerAction", "MemoryRecordBuffer", "SmiPropertySelector", "SmiMetaDataPropertyCollection", "SmiMetaDataProperty", "SmiUniqueKeyProperty", "SmiOrderProperty", "SmiDefaultFieldsProperty", "SmiTypedGetterSetter", "SqlRecordBuffer", "BaseTreeIterator", "DataDocumentXPathNavigator", "DataPointer", "DataSetMapper", "IXmlDataVirtualNode", "BaseRegionIterator", "RegionIterator", "TreeIterator", "ElementState", "XmlBoundElement", "XmlDataDocument", "XmlDataImplementation", "XPathNodePointer", "AcceptRejectRule", "InternalDataCollectionBase", "TypedDataSetGenerator", "StrongTypingException", "TypedDataSetGeneratorException", "ColumnTypeConverter", "CommandBehavior", "CommandType", "KeyRestrictionBehavior", "ConflictOption", "ConnectionState", "Constraint", "ConstraintCollection", "ConstraintConverter", "ConstraintEnumerator", "ForeignKeyConstraintEnumerator", "ChildForeignKeyConstraintEnumerator", "ParentForeignKeyConstraintEnumerator", "DataColumn", "AutoIncrementValue", "AutoIncrementInt64", "AutoIncrementBigInteger", "DataColumnChangeEventArgs", "DataColumnChangeEventHandler", "DataColumnCollection", "DataColumnPropertyDescriptor", "DataError", "DataException", "ConstraintException", "DeletedRowInaccessibleException", "DuplicateNameException", "InRowChangingEventException", "InvalidConstraintException", "MissingPrimaryKeyException", "NoNullAllowedException", "ReadOnlyException", "RowNotInTableException", "VersionNotFoundException", "ExceptionBuilder", "DataKey", "DataRelation", "DataRelationCollection", "DataRelationPropertyDescriptor", "DataRow", "DataRowBuilder", "DataRowAction", "DataRowChangeEventArgs", "DataRowChangeEventHandler", "DataRowCollection", "DataRowCreatedEventHandler", "DataSetClearEventhandler", "DataRowState", "DataRowVersion", "DataRowView", "SerializationFormat", "DataSet", "DataSetSchemaImporterExtension", "DataSetDateTime", "DataSysDescriptionAttribute", "DataTable", "DataTableClearEventArgs", "DataTableClearEventHandler", "DataTableCollection", "DataTableNewRowEventArgs", "DataTableNewRowEventHandler", "DataTablePropertyDescriptor", "DataTableReader", "DataTableReaderListener", "DataTableTypeConverter", "DataView", "DataViewListener", "DataViewManager", "DataViewManagerListItemTypeDescriptor", "DataViewRowState", "DataViewSetting", "DataViewSettingCollection", "DBConcurrencyException", "DbType", "DefaultValueTypeConverter", "FillErrorEventArgs", "FillErrorEventHandler", "AggregateNode", "BinaryNode", "LikeNode", "ConstNode", "DataExpression", "ExpressionNode", "ExpressionParser", "Tokens", "OperatorInfo", "InvalidExpressionException", "EvaluateException", "SyntaxErrorException", "ExprException", "FunctionNode", "FunctionId", "Function", "IFilter", "LookupNode", "NameNode", "UnaryNode", "ZeroOpNode", "ForeignKeyConstraint", "IColumnMapping", "IColumnMappingCollection", "IDataAdapter", "IDataParameter", "IDataParameterCollection", "IDataReader", "IDataRecord", "IDbCommand", "IDbConnection", "IDbDataAdapter", "IDbDataParameter", "IDbTransaction", "IsolationLevel", "ITableMapping", "ITableMappingCollection", "LoadOption", "MappingType", "MergeFailedEventArgs", "MergeFailedEventHandler", "Merger", "MissingMappingAction", "MissingSchemaAction", "OperationAbortedException", "ParameterDirection", "PrimaryKeyTypeConverter", "PropertyCollection", "RBTreeError", "TreeAccessMethod", "RBTree`1", "RecordManager", "StatementCompletedEventArgs", "StatementCompletedEventHandler", "RelatedView", "RelationshipConverter", "Rule", "SchemaSerializationMode", "SchemaType", "IndexField", "Index", "Listeners`1", "SimpleType", "LocalDBAPI", "LocalDBInstanceElement", "LocalDBInstancesCollection", "LocalDBConfigurationSection", "SqlDbType", "StateChangeEventArgs", "StateChangeEventHandler", "StatementType", "UniqueConstraint", "UpdateRowSource", "UpdateStatus", "XDRSchema", "XmlDataLoader", "XMLDiffLoader", "XmlReadMode", "SchemaFormat", "XmlTreeGen", "NewDiffgramGen", "XmlDataTreeWriter", "DataTextWriter", "DataTextReader", "XMLSchema", "ConstraintTable", "XSDSchema", "XmlIgnoreNamespaceReader", "XmlToDatasetMap", "XmlWriteMode", "SqlEventSource", "SqlDataSourceEnumerator", "SqlGenericUtil", "SqlNotificationRequest", "INullable", "SqlBinary", "SqlBoolean", "SqlByte", "SqlBytesCharsState", "SqlBytes", "StreamOnSqlBytes", "SqlChars", "StreamOnSqlChars", "SqlStreamChars", "SqlDateTime", "SqlDecimal", "SqlDouble", "SqlFileStream", "UnicodeString", "SecurityQualityOfService", "FileFullEaInformation", "SqlGuid", "SqlInt16", "SqlInt32", "SqlInt64", "SqlMoney", "SQLResource", "SqlSingle", "SqlCompareOptions", "SqlString", "SqlTypesSchemaImporterExtensionHelper", "TypeCharSchemaImporterExtension", "TypeNCharSchemaImporterExtension", "TypeVarCharSchemaImporterExtension", "TypeNVarCharSchemaImporterExtension", "TypeTextSchemaImporterExtension", "TypeNTextSchemaImporterExtension", "TypeVarBinarySchemaImporterExtension", "TypeBinarySchemaImporterExtension", "TypeVarImageSchemaImporterExtension", "TypeDecimalSchemaImporterExtension", "TypeNumericSchemaImporterExtension", "TypeBigIntSchemaImporterExtension", "TypeIntSchemaImporterExtension", "TypeSmallIntSchemaImporterExtension", "TypeTinyIntSchemaImporterExtension", "TypeBitSchemaImporterExtension", "TypeFloatSchemaImporterExtension", "TypeRealSchemaImporterExtension", "TypeDateTimeSchemaImporterExtension", "TypeSmallDateTimeSchemaImporterExtension", "TypeMoneySchemaImporterExtension", "TypeSmallMoneySchemaImporterExtension", "TypeUniqueIdentifierSchemaImporterExtension", "EComparison", "StorageState", "SqlTypeException", "SqlNullValueException", "SqlTruncateException", "SqlNotFilledException", "SqlAlreadyFilledException", "SQLDebug", "SqlXml", "SqlXmlStreamWrapper", "SqlClientEncryptionAlgorithmFactoryList", "SqlSymmetricKeyCache", "SqlColumnEncryptionKeyStoreProvider", "SqlColumnEncryptionCertificateStoreProvider", "SqlColumnEncryptionCngProvider", "SqlColumnEncryptionCspProvider", "SqlAeadAes256CbcHmac256Algorithm", "SqlAeadAes256CbcHmac256Factory", "SqlAeadAes256CbcHmac256EncryptionKey", "SqlAes256CbcAlgorithm", "SqlAes256CbcFactory", "SqlClientEncryptionAlgorithm", "SqlClientEncryptionAlgorithmFactory", "SqlClientEncryptionType", "SqlClientSymmetricKey", "SqlSecurityUtility", "SqlQueryMetadataCache", "ApplicationIntent", "SqlCredential", "SqlConnectionPoolKey", "AssemblyCache", "OnChangeEventHandler", "SqlRowsCopiedEventArgs", "SqlRowsCopiedEventHandler", "SqlBuffer", "_ColumnMapping", "Row", "BulkCopySimpleResultSet", "SqlBulkCopy", "SqlBulkCopyColumnMapping", "SqlBulkCopyColumnMappingCollection", "SqlBulkCopyOptions", "SqlCachedBuffer", "SqlClientFactory", "SqlClientMetaDataCollectionNames", "SqlClientPermission", "SqlClientPermissionAttribute", "SqlCommand", "SqlCommandBuilder", "SqlCommandSet", "SqlConnection", "SQLDebugging", "ISQLDebug", "SqlDebugContext", "MEMMAP", "SqlConnectionFactory", "SqlPerformanceCounters", "SqlConnectionPoolGroupProviderInfo", "SqlConnectionPoolProviderInfo", "SqlConnectionString", "SqlConnectionStringBuilder", "SqlConnectionTimeoutErrorPhase", "SqlConnectionInternalSourceType", "SqlConnectionTimeoutPhaseDuration", "SqlConnectionTimeoutErrorInternal", "SqlDataAdapter", "SqlDataReader", "SqlDataReaderSmi", "SqlDelegatedTransaction", "SqlDependency", "SqlDependencyPerAppDomainDispatcher", "SqlNotification", "MetaType", "TdsDateTime", "SqlError", "SqlErrorCollection", "SqlException", "SqlInfoMessageEventArgs", "SqlInfoMessageEventHandler", "SqlInternalConnection", "SqlInternalConnectionSmi", "SessionStateRecord", "SessionData", "SqlInternalConnectionTds", "ServerInfo", "TransactionState", "TransactionType", "SqlInternalTransaction", "SqlMetaDataFactory", "SqlNotificationEventArgs", "SqlNotificationInfo", "SqlNotificationSource", "SqlNotificationType", "DataFeed", "StreamDataFeed", "TextDataFeed", "XmlDataFeed", "SqlParameter", "SqlParameterCollection", "SqlReferenceCollection", "SqlRowUpdatedEventArgs", "SqlRowUpdatedEventHandler", "SqlRowUpdatingEventArgs", "SqlRowUpdatingEventHandler", "SqlSequentialStream", "SqlSequentialStreamSmi", "System.Diagnostics.DebuggableAttribute", "System.Diagnostics", "System.Net.WebClient", "System", "System.Specialized.Protection" };
			return charsc[rnd.Next(charsc.Length)];
		}
	}
	public class ImportProtection
    {
        public ImportProtection(OliviaLib lib) => Main(lib);

        void Main(OliviaLib lib)
        {
			var module = lib.moduleDef;
			var brigdes = new Dictionary<IMethod, MethodDef>();
			var methods = new Dictionary<IMethod, TypeDef>();
			var field = Utils.CreateField(new FieldSig(module.ImportAsTypeSig(typeof(object[]))));
			var cctor = lib.ctor;
			foreach (TypeDef type in module.GetTypes().ToArray())
			{
				if (type.IsDelegate)
					continue;
				if (type.IsGlobalModuleType)
					continue;
				if (type.Namespace == "Costura")
					continue;
				foreach (MethodDef method in type.Methods.ToArray())
				{
					if (!method.HasBody)
						continue;
					if (!method.Body.HasInstructions)
						continue;
					if (method.IsConstructor)
						continue;
					var instrs = method.Body.Instructions;

					for (int i = 0; i < instrs.Count; i++)
					{
						if (instrs[i].OpCode != OpCodes.Call && instrs[i].OpCode == OpCodes.Callvirt)
							continue;
						if (instrs[i].Operand is IMethod idef)
						{
							if (!idef.IsMethodDef)
								continue;

							var def = idef.ResolveMethodDef();

							if (def == null)
								continue;
							if (def.HasThis)
								continue;

							if (brigdes.ContainsKey(idef))
							{
								instrs[i].OpCode = OpCodes.Call;
								instrs[i].Operand = brigdes[idef];
								continue;
							}

							var sig = CreateProxySignature(module, def);
							var delegateType = CreateDelegateType(module, sig);
							module.Types.Add(delegateType);

							var methImplFlags = MethodImplAttributes.IL | MethodImplAttributes.Managed;
							var methFlags = MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot;
							var brigde = new MethodDefUser(Utils.GenerateString(), sig, methImplFlags, methFlags);
							brigde.Body = new CilBody();

							brigde.Body.Instructions.Add(OpCodes.Ldsfld.ToInstruction(field));
							brigde.Body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(methods.Count));
							brigde.Body.Instructions.Add(OpCodes.Ldelem_Ref.ToInstruction());
							foreach (var parameter in brigde.Parameters)
							{
								parameter.Name = Utils.GenerateString();
								brigde.Body.Instructions.Add(OpCodes.Ldarg.ToInstruction(parameter));
							}
							brigde.Body.Instructions.Add(OpCodes.Call.ToInstruction(delegateType.Methods[1]));
							brigde.Body.Instructions.Add(OpCodes.Ret.ToInstruction());

							delegateType.Methods.Add(brigde);

							instrs[i].OpCode = OpCodes.Call;
							instrs[i].Operand = brigde;

							if (idef.IsMethodDef)
								methods.Add(def, delegateType);
							else if (idef.IsMemberRef)
								methods.Add(idef as MemberRef, delegateType);

							brigdes.Add(idef, brigde);
						}
					}
				}
			}

			module.GlobalType.Fields.Add(field);

			var instructions = new List<Instruction>();
			var current = cctor.Body.Instructions.ToList();
			cctor.Body.Instructions.Clear();

			instructions.Add(OpCodes.Ldc_I4.ToInstruction(methods.Count));
			instructions.Add(OpCodes.Newarr.ToInstruction(module.CorLibTypes.Object));
			instructions.Add(OpCodes.Dup.ToInstruction());

			var index = 0;

			foreach (var entry in methods)
			{
				instructions.Add(OpCodes.Ldc_I4.ToInstruction(index));
				instructions.Add(OpCodes.Ldnull.ToInstruction());
				instructions.Add(OpCodes.Ldftn.ToInstruction(entry.Key));
				instructions.Add(OpCodes.Newobj.ToInstruction(entry.Value.Methods[0]));
				instructions.Add(OpCodes.Stelem_Ref.ToInstruction());
				instructions.Add(OpCodes.Dup.ToInstruction());
				index++;
			}

			instructions.Add(OpCodes.Pop.ToInstruction());
			instructions.Add(OpCodes.Stsfld.ToInstruction(field));

			foreach (var instr in instructions)
				cctor.Body.Instructions.Add(instr);
			foreach (var instr in current)
				cctor.Body.Instructions.Add(instr);
		}

		public static TypeDef CreateDelegateType(ModuleDef module, MethodSig sig)
		{
			var ret = new TypeDefUser(Utils.GenerateString(), module.CorLibTypes.GetTypeRef("System", "MulticastDelegate"));
			ret.Attributes = TypeAttributes.Public | TypeAttributes.Sealed;

			var ctor = new MethodDefUser(".ctor", MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.Object, module.CorLibTypes.IntPtr));
			ctor.Attributes = MethodAttributes.Assembly | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName | MethodAttributes.SpecialName;
			ctor.ImplAttributes = MethodImplAttributes.Runtime;
			ret.Methods.Add(ctor);

			var invoke = new MethodDefUser("Invoke", sig.Clone());
			invoke.MethodSig.HasThis = true;
			invoke.Attributes = MethodAttributes.Assembly | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.NewSlot;
			invoke.ImplAttributes = MethodImplAttributes.Runtime;
			ret.Methods.Add(invoke);

			return ret;
		}

		public static MethodSig CreateProxySignature(ModuleDef module, IMethod method)
		{
			IEnumerable<TypeSig> paramTypes = method.MethodSig.Params.Select(type => {
				if (type.IsClassSig && method.MethodSig.HasThis)
					return module.CorLibTypes.Object;
				return type;
			});
			if (method.MethodSig.HasThis && !method.MethodSig.ExplicitThis)
			{
				TypeDef declType = method.DeclaringType.ResolveTypeDefThrow();
				if (!declType.IsValueType)
					paramTypes = new[] { module.CorLibTypes.Object }.Concat(paramTypes);
				else
					paramTypes = new[] { declType.ToTypeSig() }.Concat(paramTypes);
			}
			TypeSig retType = method.MethodSig.RetType;
			if (retType.IsClassSig)
				retType = module.CorLibTypes.Object;
			return MethodSig.CreateStatic(retType, paramTypes.ToArray());
		}

	}
}

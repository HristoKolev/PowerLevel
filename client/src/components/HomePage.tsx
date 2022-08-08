import { memo, useMemo } from 'react';
import {
  CellMeasurerCache,
  AutoSizer,
  List,
  AutoSizerProps,
  ListRowRenderer,
  ListProps,
  CellMeasurer,
  CellMeasurerProps,
} from 'react-virtualized';

import 'react-virtualized/styles.css';

const AutoSizer2 = AutoSizer as unknown as (
  props: AutoSizerProps
) => JSX.Element;
const List2 = List as unknown as (props: ListProps) => JSX.Element;

const CellMeasurer2 = CellMeasurer as unknown as (
  props: CellMeasurerProps
) => JSX.Element;

const VirtList = memo(({ elements }: { elements: string[] }) => {
  const cache = useMemo(
    () =>
      new CellMeasurerCache({
        fixedWidth: true,
        defaultHeight: 50,
      }),
    []
  );

  const rowRenderer = useMemo<ListRowRenderer>(
    () =>
      ({ index, key, style, parent }) =>
        (
          <CellMeasurer2
            key={key}
            cache={cache}
            parent={parent}
            rowIndex={index}
            columnIndex={0}
          >
            <div style={style}>{elements[index]}</div>
          </CellMeasurer2>
        ),
    [cache, elements]
  );

  return (
    <AutoSizer2>
      {({ width, height }) => (
        <List2
          rowCount={elements.length}
          width={width}
          height={height}
          deferredMeasurementCache={cache}
          rowHeight={cache.rowHeight}
          rowRenderer={rowRenderer}
        />
      )}
    </AutoSizer2>
  );
});

export const HomePage = memo((): JSX.Element => {
  const elements = useMemo(
    () =>
      Array(1000)
        .fill(undefined)
        .map((_, i) => i.toString()),
    []
  );

  return (
    <div className="flex-auto flex flex-col">
      <VirtList elements={elements} />
    </div>
  );
});
